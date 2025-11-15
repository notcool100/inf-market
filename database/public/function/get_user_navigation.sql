-- DROP FUNCTION public.get_user_navigation(uuid);

CREATE OR REPLACE FUNCTION public.get_user_navigation(p_user_id uuid)
 RETURNS TABLE(id uuid, label text, icon text, url text, "order" integer, "parentId" uuid, "groupId" uuid, "groupName" text, "groupOrder" integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT DISTINCT
        ni.id,
        ni.label,
        ni.icon,
        ni.url,
        ni."order",
        ni."parentId",
        ni."groupId",
        ng."name" as "groupName",
        ng."order" as "groupOrder"
    FROM navigation_items ni
    INNER JOIN role_navigation rn ON ni.id = rn."navigationItemId"
    INNER JOIN UserRoles ur ON rn."roleId" = ur.RoleId
    LEFT JOIN navigation_groups ng ON ni."groupId" = ng.id
    WHERE ur.UserId = p_user_id
        AND ni."isActive" = true
        AND (ng."isActive" = true OR ng."isActive" IS NULL)
    ORDER BY ng."order", ni."order";
END;
$function$
;
