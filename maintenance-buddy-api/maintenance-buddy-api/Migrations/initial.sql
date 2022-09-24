CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "UserVehicleConnections" (
    "NameIdentifier" text NOT NULL,
    "VehicleId" uuid NOT NULL,
    CONSTRAINT "PK_UserVehicleConnections" PRIMARY KEY ("NameIdentifier", "VehicleId")
);

CREATE TABLE "Vehicles" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "Kilometer" integer NOT NULL,
    CONSTRAINT "PK_Vehicles" PRIMARY KEY ("Id")
);

CREATE TABLE "ActionTemplate" (
    "Id" uuid NOT NULL,
    "Name" text NOT NULL,
    "KilometerInterval" integer NOT NULL,
    "TimeInterval" interval NOT NULL,
    "VehicleId" uuid NULL,
    CONSTRAINT "PK_ActionTemplate" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_ActionTemplate_Vehicles_VehicleId" FOREIGN KEY ("VehicleId") REFERENCES "Vehicles" ("Id")
);

CREATE TABLE "MaintenanceAction" (
    "Id" uuid NOT NULL,
    "Kilometer" integer NOT NULL,
    "ActionTemplateId" uuid NOT NULL,
    "Date" timestamp with time zone NOT NULL,
    "Note" text NOT NULL,
    CONSTRAINT "PK_MaintenanceAction" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_MaintenanceAction_ActionTemplate_ActionTemplateId" FOREIGN KEY ("ActionTemplateId") REFERENCES "ActionTemplate" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_ActionTemplate_VehicleId" ON "ActionTemplate" ("VehicleId");

CREATE INDEX "IX_MaintenanceAction_ActionTemplateId" ON "MaintenanceAction" ("ActionTemplateId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220924141405_initial', '6.0.8');

COMMIT;

