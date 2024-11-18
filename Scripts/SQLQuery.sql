CREATE DATABASE GestionEventos;

USE GestionEventos;

CREATE TABLE Usuarios (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100),
    CorreoElectronico VARCHAR(100) UNIQUE,
    Contrase�a VARCHAR(100)
);

CREATE TABLE Eventos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(100),
    Descripcion TEXT,
    FechaHora DATETIME,
    Ubicacion VARCHAR(255),
    CapacidadMaxima INT,
    UsuarioId INT,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

CREATE TABLE Inscripciones (
    Id INT PRIMARY KEY IDENTITY(1,1),
    EventoId INT,
    UsuarioId INT,
    FOREIGN KEY (EventoId) REFERENCES Eventos(Id),
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
);

-- Insertar registros en la tabla Usuarios
INSERT INTO Usuarios (Nombre, CorreoElectronico, Contraseña)
VALUES 
('Juan Pérez', 'juan.perez@example.com', 'contraseña123'),
('María López', 'maria.lopez@example.com', 'segura456'),
('Carlos Gómez', 'carlos.gomez@example.com', 'clave789');

-- Insertar registros en la tabla Eventos
INSERT INTO Eventos (Nombre, Descripcion, FechaHora, Ubicacion, CapacidadMaxima, UsuarioId)
VALUES 
('Conferencia Tech 2024', 'Conferencia sobre tecnología y tendencias 2024.', '2024-12-01 10:00:00', 'Centro de Convenciones', 200, 1),
('Taller de Innovación', 'Un taller para aprender técnicas innovadoras.', '2024-12-05 14:00:00', 'Sala de Reuniones A', 50, 2),
('Fiesta de Fin de Año', 'Celebración de fin de año para todos los empleados.', '2024-12-31 20:00:00', 'Salón Principal', 100, 3);

-- Insertar registros en la tabla Inscripciones
INSERT INTO Inscripciones (EventoId, UsuarioId)
VALUES 
(1, 1), -- Juan se inscribe en la Conferencia Tech
(2, 2), -- María se inscribe en el Taller de Innovación
(3, 3), -- Carlos se inscribe en la Fiesta de Fin de Año
(1, 2), -- María también se inscribe en la Conferencia Tech
(2, 1), -- Juan se inscribe en el Taller de Innovación
(3, 1); -- Juan también se inscribe en la Fiesta de Fin de Año

CREATE PROCEDURE ObtenerEventos
AS
BEGIN
    SELECT
        E.Id,
        E.Nombre,
        E.Descripcion,
        E.FechaHora,
        E.Ubicacion,
        E.CapacidadMaxima,
        U.Id AS UsuarioId,
        U.Nombre AS UsuarioNombre,
        (SELECT COUNT(*) FROM Inscripciones I WHERE I.EventoId = E.Id) AS AsistentesRegistrados
    FROM Eventos E
    INNER JOIN Usuarios U ON E.UsuarioId = U.Id;
END

CREATE PROCEDURE ObtenerEvento
    @id INT = NULL
AS
BEGIN
    -- Obtener evento espec�fico por ID
    SELECT
        E.Id,
        E.Nombre,
        E.Descripcion,
        E.FechaHora,
        E.Ubicacion,
        E.CapacidadMaxima,
		U.Id AS UsuarioId,
        U.Nombre AS UsuarioNombre,
        (SELECT COUNT(*) FROM Inscripciones I WHERE I.EventoId = E.Id) AS AsistentesRegistrados
        FROM Eventos E
    INNER JOIN Usuarios U ON E.UsuarioId = U.Id
    WHERE E.Id = @id;
END

CREATE PROCEDURE CrearEvento
    @pNombre VARCHAR(100),
    @pDescripcion TEXT,
    @pFechaHora DATETIME,
    @pUbicacion VARCHAR(255),
    @pCapacidadMaxima INT,
    @pUsuarioId INT
AS
BEGIN
    INSERT INTO Eventos (Nombre, Descripcion, FechaHora, Ubicacion, CapacidadMaxima, UsuarioId)
    VALUES (@pNombre, @pDescripcion, @pFechaHora, @pUbicacion, @pCapacidadMaxima, @pUsuarioId);
END;

CREATE PROCEDURE EditarEvento
    @pEventoId INT,
    @pCapacidadMaxima INT,
    @pFechaHora DATETIME,
    @pUbicacion VARCHAR(255),
    @pUsuarioId INT
AS
BEGIN
    UPDATE Eventos
    SET CapacidadMaxima = @pCapacidadMaxima,
        FechaHora = @pFechaHora,
        Ubicacion = @pUbicacion,
        UsuarioId = @pUsuarioId
    WHERE Id = @pEventoId;
END;


CREATE PROCEDURE EliminarEvento
    @pEventoId INT
AS
BEGIN
    DECLARE @asistentesCount INT;

    SELECT @asistentesCount = COUNT(*)
    FROM Inscripciones
    WHERE EventoId = @pEventoId;

    IF @asistentesCount = 0
    BEGIN
        DELETE FROM Eventos WHERE Id = @pEventoId;
    END;
END;

CREATE PROCEDURE ObtenerUsuarios
AS
BEGIN
    SELECT
        U.Id,
        U.Nombre,
        U.CorreoElectronico
    FROM Usuarios U;
END;

CREATE PROCEDURE ObtenerUsuario
    @id INT
AS
BEGIN
    SELECT
        U.Id,
        U.Nombre,
        U.CorreoElectronico
    FROM Usuarios U
    WHERE U.Id = @id;
END;

CREATE PROCEDURE InscribirseEnEvento
    @pEventoId INT,
    @pUsuarioId INT
AS
BEGIN
    DECLARE @capacidadMaxima INT;
    DECLARE @asistentesCount INT;
    DECLARE @inscripcionesCount INT;

    -- Verificar si el usuario es el creador del evento
    IF EXISTS (SELECT 1 FROM Eventos WHERE Id = @pEventoId AND UsuarioId = @pUsuarioId)
    BEGIN
        RAISERROR('No puedes inscribirte en tu propio evento.', 16, 1);
        RETURN;
    END;

    -- Verificar la capacidad máxima del evento
    SELECT @capacidadMaxima = CapacidadMaxima
    FROM Eventos
    WHERE Id = @pEventoId;

    SELECT @asistentesCount = COUNT(*)
    FROM Inscripciones
    WHERE EventoId = @pEventoId;

    IF @asistentesCount >= @capacidadMaxima
    BEGIN
        RAISERROR('El evento ha alcanzado su capacidad máxima.', 16, 1);
        RETURN;
    END;

    -- Verificar el número máximo de inscripciones del usuario
    SELECT @inscripcionesCount = COUNT(*)
    FROM Inscripciones
    WHERE UsuarioId = @pUsuarioId;

    IF @inscripcionesCount >= 3
    BEGIN
        RAISERROR('Has alcanzado el máximo de inscripciones permitidas.', 16, 1);
        RETURN;
    END;

    -- Inscribir al usuario en el evento
    INSERT INTO Inscripciones (EventoId, UsuarioId)
    VALUES (@pEventoId, @pUsuarioId);
END;