-- Create RolePermissions table
CREATE TABLE IF NOT EXISTS role_permissions (
    id UUID NOT NULL,
    "roleId" UUID NOT NULL,
    "permissionId" TEXT NOT NULL,
    "createdAt" TIMESTAMP(3) DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT role_permissions_pkey PRIMARY KEY (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS "role_permissions_roleId_permissionId_key" 
    ON role_permissions USING btree ("roleId", "permissionId");

ALTER TABLE role_permissions 
    ADD CONSTRAINT "role_permissions_permissionId_fkey" 
    FOREIGN KEY ("permissionId") REFERENCES permissions(id) 
    ON DELETE RESTRICT ON UPDATE CASCADE;

ALTER TABLE role_permissions 
    ADD CONSTRAINT "role_permissions_roleId_fkey" 
    FOREIGN KEY ("roleId") REFERENCES Roles(id) 
    ON DELETE RESTRICT ON UPDATE CASCADE;

