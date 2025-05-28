-- Drop procedures if exist
BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE G_sp_GetAuthenticatedUser';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE G_sp_GetSetMenu';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP PROCEDURE G_sp_log_audit_trail';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

-- Drop tables if exist (drop child tables first due to FK dependencies)
BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_USERROLES CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_ROLEMENUFEATUREACCESS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_ROLEMENUACCESS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_MENUFEATURES CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_MENUS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_MENUFEATUREMASTER CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_ROLES CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_USERS CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_CONFIGURATION CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_AUDITTRAIL CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/


BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_PasswordResetTokens CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/

BEGIN
  EXECUTE IMMEDIATE 'DROP TABLE G_EmailNotification CASCADE CONSTRAINTS';
EXCEPTION WHEN OTHERS THEN NULL;
END;
/


COMMIT;

-- Create tables

-- 1. Audit Trail
CREATE TABLE G_AUDITTRAIL
(
  ID             NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  TABLE_NAME     VARCHAR2(255 BYTE),
  RECORD_ID      VARCHAR2(255 BYTE),
  OPERATION      VARCHAR2(10 BYTE)              DEFAULT NULL,
  CHANGED_DATA   CLOB                           DEFAULT NULL,
  PREVIOUS_DATA  CLOB                           DEFAULT NULL,
  CHANGED_BY     VARCHAR2(255 BYTE)             DEFAULT NULL,
  CHANGED_AT     TIMESTAMP(6)                   DEFAULT CURRENT_TIMESTAMP,
  IP_ADDRESS     VARCHAR2(45 BYTE)              DEFAULT NULL,
  REMARKS        CLOB                           DEFAULT NULL
);

-- 2. Configuration (no primary key or unique specified)
CREATE TABLE G_CONFIGURATION
(
  CONFIG_KEY    VARCHAR2(255 BYTE),
  CONFIG_VALUE  VARCHAR2(255 BYTE)
);

-- 3. Menu Feature Master
CREATE TABLE G_MENUFEATUREMASTER
(
  FEATUREID           NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  FEATURENAME         VARCHAR2(100 BYTE),
  FEATURECODE         VARCHAR2(50 BYTE),
  FEATUREDESCRIPTION  VARCHAR2(255 BYTE),
  CREATEDBY           NUMBER(10),
  CREATEDDATE         DATE,
  MODIFIEDBY          NUMBER(10),
  MODIFIEDDATE        DATE
);

-- 5. Menus (create before G_MENUFEATURES as FK references it)
CREATE TABLE G_MENUS
(
  MENUID          NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  PARENTID        NUMBER(10)                    DEFAULT 0,
  MENUNAME        NVARCHAR2(100),
  AREA            NVARCHAR2(50),
  CONTROLLERNAME  NVARCHAR2(50),
  ACTIONNAME      NVARCHAR2(50),
  URL             VARCHAR2(255),
  "Order"         NUMBER(10),
  ISACTIVE        NUMBER(1)                     DEFAULT 0,
  ICONCLASS       VARCHAR2(30 BYTE),
  CREATEDBY       NUMBER(10)                    DEFAULT 0,
  CREATEDDATE     DATE,
  MODIFIEDBY      NUMBER(10),
  MODIFIEDDATE    DATE
);

-- 4. Menu Features (depends on G_MENUS and G_MENUFEATUREMASTER)
CREATE TABLE G_MENUFEATURES
(
  MENUFEATUREID  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  MENUID         NUMBER,
  FEATUREID      NUMBER,
  CREATEDBY      NUMBER,
  CREATEDDATE    DATE                           DEFAULT SYSDATE,
  MODIFIEDBY     NUMBER,
  MODIFIEDDATE   DATE,
  FEATUREURL     VARCHAR2(255 BYTE),
  CONSTRAINT FK_MENUFEATURES_MENUID FOREIGN KEY (MENUID) REFERENCES G_MENUS(MENUID),
  CONSTRAINT FK_MENUFEATURES_FEATUREID FOREIGN KEY (FEATUREID) REFERENCES G_MENUFEATUREMASTER(FEATUREID)
);

-- 8. Roles
CREATE TABLE G_ROLES
(
  ROLEID        NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  ROLENAME      NVARCHAR2(100),
  DESCRIPTION   NVARCHAR2(255),
  ISACTIVE      NUMBER(1)                       DEFAULT 1,
  CREATEDBY     NUMBER(10),
  CREATEDDATE   DATE                            DEFAULT CURRENT_DATE,
  MODIFIEDBY    NUMBER(10),
  MODIFIEDDATE  DATE,
  CONSTRAINT UQ_G_ROLES_ROLENAME UNIQUE (ROLENAME)
);

-- 9. Users
CREATE TABLE G_USERS
(
  USERID                   NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  CUSTOMERID               VARCHAR2(50 BYTE),
  COMPANYCODE              NUMBER(2),
  STATUS                   VARCHAR2(2 BYTE),
  FIRSTNAME                VARCHAR2(65 BYTE),
  LASTNAME                 VARCHAR2(50 BYTE),
  USERNAME                 NVARCHAR2(100),
  EMAIL                    NVARCHAR2(100),
  MOBILE                   VARCHAR2(15),
  DOB                      DATE,
  PASSWORDHASH             NVARCHAR2(255),
  OTP                      VARCHAR2(50),
  OTPCREATEDDATE           DATE,
  ISRESENDOTP              NUMBER(1),
  RESENDOTPDATE            DATE,
  LASTLOGINDATE            DATE,
  ISFIRSTTIMELOGIN         NUMBER(1),
  PASSWORDLASTCHANGEDDATE  DATE,
  ISACTIVE                 NUMBER(1)            DEFAULT 1,
  CREATEDBY                NUMBER(10),
  CREATEDDATE              TIMESTAMP(6)         DEFAULT CURRENT_TIMESTAMP,
  MODIFIEDBY               NUMBER(10),
  MODIFIEDDATE             TIMESTAMP(6),
  FILEPATH                 NVARCHAR2(255),
  CUSTOMERTYPE             CHAR(1 BYTE),
  CONSTRAINT UQ_G_USERS_USERNAME UNIQUE (USERNAME),
  CONSTRAINT UQ_G_USERS_EMAIL UNIQUE (EMAIL),
  CONSTRAINT UQ_G_USERS_CUSTOMERID UNIQUE (CUSTOMERID)
);

-- 6. Role Menu Access (depends on G_ROLES and G_MENUS)
CREATE TABLE G_ROLEMENUACCESS
(
  ROLEMENUACCESSID  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  ROLEID            NUMBER(10),
  MENUID            NUMBER(10),
  CREATEDBY         NUMBER(10),
  CREATEDDATE       DATE                        DEFAULT CURRENT_DATE,
  MODIFIEDBY        NUMBER(10),
  MODIFIEDDATE      DATE,
  ISACCESSIBLE      NUMBER(1)                   DEFAULT 1,
  CONSTRAINT FK_ROLEMENUACCESS_ROLEID FOREIGN KEY (ROLEID) REFERENCES G_ROLES(ROLEID),
  CONSTRAINT FK_ROLEMENUACCESS_MENUID FOREIGN KEY (MENUID) REFERENCES G_MENUS(MENUID)
);

-- 7. Role Menu Feature Access (depends on G_ROLES and G_MENUFEATURES)
CREATE TABLE G_ROLEMENUFEATUREACCESS
(
  ROLEMENUFEATUREACCESSID  NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  ROLEID                   NUMBER(10),
  MENUFEATUREID            NUMBER(10),
  ISACTIVE                 NUMBER(1)            DEFAULT 1,
  CREATEDBY                NUMBER(10),
  CREATEDDATE              DATE                 DEFAULT SYSDATE,
  MODIFIEDBY               NUMBER(10),
  MODIFIEDDATE             DATE,
  CONSTRAINT FK_ROLEMENUFEATURE_ROLEID FOREIGN KEY (ROLEID) REFERENCES G_ROLES(ROLEID),
  CONSTRAINT FK_ROLEMENUFEATURE_MENUFEATUREID FOREIGN KEY (MENUFEATUREID) REFERENCES G_MENUFEATURES(MENUFEATUREID)
);

-- 10. User Roles (one role per user constraint)
CREATE TABLE G_USERROLES
(
  USERID        NUMBER(10) PRIMARY KEY,
  ROLEID        NUMBER(10),
  ASSIGNEDDATE  DATE                            DEFAULT SYSDATE,
  CONSTRAINT FK_USERROLES_USERID FOREIGN KEY (USERID) REFERENCES G_USERS(USERID) ON DELETE CASCADE,
  CONSTRAINT FK_USERROLES_ROLEID FOREIGN KEY (ROLEID) REFERENCES G_ROLES(ROLEID) ON DELETE CASCADE
);


-- 11. Password Reset Tokens
CREATE TABLE G_PasswordResetTokens (
    TokenID            NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    UserID             NUMBER NOT NULL,
    UserName           VARCHAR2(20 CHAR) NOT NULL,
    ResetTokenHash     VARCHAR2(255 CHAR) NOT NULL,
    ExpirationTimeUTC  TIMESTAMP WITH TIME ZONE NOT NULL,
    IsUsed             NUMBER(1) DEFAULT 0 CHECK (IsUsed IN (0, 1)),
    CreatedDate        TIMESTAMP WITH TIME ZONE DEFAULT SYSTIMESTAMP AT TIME ZONE 'UTC',

    -- Constraints
    CONSTRAINT uq_ResetTokenHash UNIQUE (ResetTokenHash),
    CONSTRAINT fk_UserID FOREIGN KEY (UserID)
        REFERENCES G_Users (UserID)
        ON DELETE CASCADE
);


CREATE TABLE G_EmailNotification (
    ID              NUMBER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    Token           VARCHAR2(255 CHAR),
    ToEmailIds      VARCHAR2(1024 CHAR),
    EmailSubject    VARCHAR2(255 CHAR),
    EmailBody       CLOB,
    HasAttachment   NUMBER(1) DEFAULT 0 CHECK (HasAttachment IN (0, 1)),
    IsSent          NUMBER(1) DEFAULT 0 CHECK (IsSent IN (0, 1)),
    CreatedDate     TIMESTAMP(6),
    SentDate        TIMESTAMP(6),
    Status          CHAR(1 CHAR),
    IsHTML          NUMBER(1) DEFAULT 0 CHECK (IsHTML IN (0, 1))
);


COMMIT;


CREATE OR REPLACE PROCEDURE G_SP_GetAuthenticatedUser (
    p_userid        IN VARCHAR2,
    p_password      IN VARCHAR2,
    ret             OUT NUMBER,
    errormsg        OUT VARCHAR2,
    p_result        OUT SYS_REFCURSOR
) AS
    v_UserId            G_Users.UserId%TYPE;
    v_IsFirstTimeLogin  G_Users.IsFirstTimeLogin%TYPE;
BEGIN
    -- Initialize outputs
    errormsg := 'Something went wrong!';
    ret := -1;

    BEGIN
        -- Try fetching the user details
        SELECT U.UserId, U.IsFirstTimeLogin
        INTO v_UserId, v_IsFirstTimeLogin
        FROM G_Users U
        WHERE U.UserName = p_userid
          AND U.PasswordHash = p_password
          AND U.IsActive = 1
          AND ROWNUM = 1;

        -- Success: determine response based on IsFirstTimeLogin
        IF v_IsFirstTimeLogin is null or v_IsFirstTimeLogin = 1 THEN
            ret := 201; -- First time login
        ELSE
            ret := 200; -- Authenticated
        END IF;

        errormsg := 'User authenticated successfully.';

        -- Return user roles and metadata
        OPEN p_result FOR
            SELECT 
                U.UserID,
                R.RoleName,
                U.UserName,
                UR.RoleID,
                U.Mobile,
                U.Email,
                U.LastLoginDate
            FROM G_Users U
            JOIN G_UserRoles UR ON U.UserId = UR.UserID
            JOIN G_Roles R ON UR.RoleId = R.RoleId
            WHERE U.UserId = v_UserId;

    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            errormsg := 'Invalid username or password.';
            ret := -1; -- Unauthorized
        WHEN OTHERS THEN
            errormsg := 'Unexpected error: ' || SQLERRM;
            ret := -1; -- Internal error
    END;

END;
/


CREATE OR REPLACE PROCEDURE G_sp_GetSetMenu(
    p_flag             IN Char DEFAULT 'G',
    p_MenuId           IN NUMBER DEFAULT 0,
    
    p_UserId           IN NUMBER DEFAULT 0,
    ret             OUT VARCHAR2,
    errormsg        OUT VARCHAR2,
    --p_Flag            IN  VARCHAR2 DEFAULT NULL,
  
    g_ResultSet       OUT SYS_REFCURSOR
)
AS
BEGIN
    IF p_flag = 'I' THEN
        
        OPEN g_ResultSet FOR
        SELECT TO_CHAR(M.MenuId) as MenuId, TO_CHAR(M.ParentId) as ParentId, M.MenuName, M.Area, M.ControllerName,
               M.ActionName, M.Url, TO_CHAR(M."Order") as  "Order",
                TO_CHAR(M.IsActive) as IsActive,
                
                M.IconClass
        FROM g_Users U
        JOIN g_UserRoles UR ON U.UserId = UR.UserId
        JOIN g_RoleMenuAccess RM ON RM.RoleId = UR.RoleId
        JOIN g_Menus M ON M.MenuId = RM.MenuId
        WHERE U.UserId = p_UserId AND M.IsActive = 1 AND RM.IsAccessible = 1
        order by "Order";
 
        ret := 200;
        errormsg := 'Menu Fetched successfully';
 
    ELSE -- Default case: G
        OPEN g_ResultSet FOR
        SELECT M.MenuId, M.ParentId, M.MenuName, M.Area, M.ControllerName,
               M.ActionName, M.Url, M."Order", M.IsActive, M.IconClass
        FROM g_Menus M
        ORDER BY M.MenuId, M."Order";
 
        ret := 200;
        errormsg := 'Menu Fetched successfully';
    END IF;
 
END G_sp_GetSetMenu;
/


CREATE OR REPLACE PROCEDURE G_sp_log_audit_trail (
    p_table_name     IN VARCHAR2,
    p_record_id      IN VARCHAR2,
    p_operation      IN VARCHAR2,
    p_changed_data   IN CLOB DEFAULT NULL,
    p_previous_data  IN CLOB DEFAULT NULL,
    p_changed_by     IN VARCHAR2 DEFAULT USER,
    p_ip_address     IN VARCHAR2 DEFAULT NULL,
    p_remarks        IN CLOB DEFAULT NULL
) AS
BEGIN
    
    DECLARE
        v_enabled VARCHAR2(10);
    BEGIN
        SELECT config_value INTO v_enabled
        FROM g_configuration
        WHERE config_key = 'AuditLogEnabled';

        IF v_enabled = '0' THEN
            RETURN;
        END IF;
    EXCEPTION
        WHEN NO_DATA_FOUND THEN
            NULL; 
    END;

    
    INSERT INTO g_audittrail (
        table_name,
        record_id,
        operation,
        changed_data,
        previous_data,
        changed_by,
        ip_address,
        remarks
    ) VALUES (
        p_table_name,
        p_record_id,
        UPPER(p_operation),
        p_changed_data,
        p_previous_data,
        p_changed_by,
        p_ip_address,
        p_remarks
    );
    commit;
EXCEPTION
    WHEN OTHERS THEN
        NULL;
END;
/



-- INSERT ROLES
INSERT INTO G_ROLES(ROLENAME, DESCRIPTION, ISACTIVE, CREATEDBY, CREATEDDATE)
VALUES ('ADMIN', 'For Admin Role - Full Access', 1, 0, SYSDATE);

INSERT INTO G_ROLES(ROLENAME, DESCRIPTION, ISACTIVE, CREATEDBY, CREATEDDATE)
VALUES ('SALESMANAGER', 'FOR Sales Manager Role', 1, 0, SYSDATE);

INSERT INTO G_ROLES(ROLENAME, DESCRIPTION, ISACTIVE, CREATEDBY, CREATEDDATE)
VALUES ('CSR', 'For CSR (Rider) Role', 1, 0, SYSDATE);

commit;


-- For testing purpose: dummy users


INSERT INTO g_USERS(CUSTOMERiD, FIRSTNAME, USERNAME, PASSWORDHASH, ISACTIVE, CREATEDDATE)
VALUES ('LDPLUP071', 'Harshit', 'LDPLUP071', 'uRMpdr6YHCKSlWS64UdGKg==', 1, sysdate);

INSERT INTO g_USERS(CUSTOMERiD, FIRSTNAME, USERNAME, PASSWORDHASH, ISACTIVE, CREATEDDATE)
VALUES ('LDPLUP070', 'Ankit', 'LDPLUP070', 'uRMpdr6YHCKSlWS64UdGKg==', 1, sysdate);

COMMIT;

INSERT INTO G_USERROLES(USERID, ROLEID, ASSIGNEDDATE)
VALUES (1,1,SYSDATE);

INSERT INTO G_USERROLES(USERID, ROLEID, ASSIGNEDDATE)
VALUES (2,2, SYSDATE);

COMMIT;
