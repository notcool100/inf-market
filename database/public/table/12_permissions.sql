-- Create Permissions table
CREATE TABLE IF NOT EXISTS permissions (
    id TEXT NOT NULL,
    code TEXT NOT NULL,
    description TEXT NULL,
    "module" TEXT NOT NULL,
    "action" TEXT NOT NULL,
    "createdAt" TIMESTAMP(3) DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "updatedAt" TIMESTAMP(3) NOT NULL,
    CONSTRAINT permissions_pkey PRIMARY KEY (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS permissions_code_key ON permissions USING btree (code);

