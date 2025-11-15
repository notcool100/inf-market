-- Create RoleNavigation table
CREATE TABLE IF NOT EXISTS role_navigation (
    id UUID NOT NULL,
    "roleId" UUID NOT NULL,
    "navigationItemId" UUID NOT NULL,
    "createdAt" TIMESTAMP(3) DEFAULT CURRENT_TIMESTAMP NOT NULL,
    CONSTRAINT role_navigation_pkey PRIMARY KEY (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS "role_navigation_roleId_navigationItemId_key" 
    ON role_navigation USING btree ("roleId", "navigationItemId");

ALTER TABLE role_navigation 
    ADD CONSTRAINT "role_navigation_navigationItemId_fkey" 
    FOREIGN KEY ("navigationItemId") REFERENCES navigation_items(id) 
    ON DELETE RESTRICT ON UPDATE CASCADE;

ALTER TABLE role_navigation 
    ADD CONSTRAINT "role_navigation_roleId_fkey" 
    FOREIGN KEY ("roleId") REFERENCES Roles(id) 
    ON DELETE RESTRICT ON UPDATE CASCADE;

