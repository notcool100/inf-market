-- Create NavigationItems table
CREATE TABLE IF NOT EXISTS navigation_items (
    id UUID NOT NULL,
    "label" TEXT NOT NULL,
    icon TEXT NULL,
    url TEXT NULL,
    "order" INT4 NOT NULL,
    "parentId" UUID NULL,
    "groupId" UUID NULL,
    "isActive" BOOLEAN DEFAULT true NOT NULL,
    "createdAt" TIMESTAMP(3) DEFAULT CURRENT_TIMESTAMP NOT NULL,
    "updatedAt" TIMESTAMP(3) NOT NULL,
    CONSTRAINT navigation_items_pkey PRIMARY KEY (id)
);

ALTER TABLE navigation_items 
    ADD CONSTRAINT "navigation_items_groupId_fkey" 
    FOREIGN KEY ("groupId") REFERENCES navigation_groups(id) 
    ON DELETE SET NULL ON UPDATE CASCADE;

ALTER TABLE navigation_items 
    ADD CONSTRAINT "navigation_items_parentId_fkey" 
    FOREIGN KEY ("parentId") REFERENCES navigation_items(id) 
    ON DELETE SET NULL ON UPDATE CASCADE;

