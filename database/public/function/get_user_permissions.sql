-- Function to get permissions for a user based on their roles
CREATE OR REPLACE FUNCTION get_user_permissions(p_user_id UUID)
RETURNS TABLE (
    id TEXT,
    code TEXT,
    description TEXT,
    "module" TEXT,
    "action" TEXT
) AS $$
BEGIN
    RETURN QUERY
    SELECT DISTINCT
        p.id,
        p.code,
        p.description,
        p."module",
        p."action"
    FROM permissions p
    INNER JOIN role_permissions rp ON p.id = rp."permissionId"
    INNER JOIN UserRoles ur ON rp."roleId" = ur."RoleId"
    WHERE ur."UserId" = p_user_id
    ORDER BY p."module", p."action";
END;
$$ LANGUAGE plpgsql;

