-- Create NavigationGroups table
CREATE TABLE IF NOT EXISTS navigation_groups (
    id UUID NOT NULL,
    "name" TEXT NOT NULL,
    description TEXT NULL,
    "order" INT4 NOT NULL,
    "isActive" BOOLEAN DEFAULT true NOT NULL,
    "createdAt" TIMESTAMP(3) DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "updatedAt" TIMESTAMP(3) NOT NULL,
    CONSTRAINT navigation_groups_pkey PRIMARY KEY (id)
);

CREATE UNIQUE INDEX IF NOT EXISTS navigation_groups_name_key 
    ON navigation_groups USING btree (name);

