-- DROP FUNCTION public.get_user_permissions(uuid);

CREATE OR REPLACE FUNCTION public.get_user_permissions(p_user_id uuid)
 RETURNS TABLE(id text, code text, description text, module text, action text)
 LANGUAGE plpgsql
AS $function$
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
    INNER JOIN UserRoles ur ON rp."roleId" = ur.roleid
    WHERE ur.UserId = p_user_id
    ORDER BY p.module, p.action;
END;
$function$
;
