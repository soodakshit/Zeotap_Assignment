CREATE TABLE IF NOT EXISTS "Incidents" (
    "Id" uuid PRIMARY KEY DEFAULT gen_random_uuid(),
    "Title" varchar(200) NOT NULL,
    "Service" varchar(100) NOT NULL,
    "Severity" varchar(50) NOT NULL,
    "Status" varchar(50) NOT NULL,
    "Owner" varchar(100),
    "Summary" varchar(2000),
    "CreatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "UpdatedAt" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IF NOT EXISTS "IX_Incidents_Service" ON "Incidents" ("Service");
CREATE INDEX IF NOT EXISTS "IX_Incidents_Severity" ON "Incidents" ("Severity");
CREATE INDEX IF NOT EXISTS "IX_Incidents_Status" ON "Incidents" ("Status");
CREATE INDEX IF NOT EXISTS "IX_Incidents_CreatedAt" ON "Incidents" ("CreatedAt");