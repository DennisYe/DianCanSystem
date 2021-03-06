/************************************
注意:最下方的默认超级管理员需要修改
************************************/


CREATE TABLE [dbo].[R_PageInfo](
	[PID] [int] IDENTITY(1,1) NOT NULL,
	[PName] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[PUrl] [nvarchar](500) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IsUrl] [bit] NOT NULL,
	[Queue] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[DefShowChild] [bit] NOT NULL CONSTRAINT [DF_R_PageInfo_DefShowChild]  DEFAULT ((0)),
 CONSTRAINT [PK_R_PageInfo] PRIMARY KEY CLUSTERED 
(
	[PID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'母菜单的排列顺序' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_PageInfo', @level2type=N'COLUMN', @level2name=N'Queue'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'-1表示是隐藏页,0 表示是根目录下面的' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_PageInfo', @level2type=N'COLUMN', @level2name=N'ParentID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'在导航中是否默认展开子项' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_PageInfo', @level2type=N'COLUMN', @level2name=N'DefShowChild'

GO

CREATE TABLE [dbo].[R_Admin](
	[AID] [int] IDENTITY(1,1) NOT NULL,
	[AName] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[APwd] [nvarchar](32) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ANickName] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[IP] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NULL,
	[Email] [nvarchar](50) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[GID] [int] NOT NULL,
	[ALastTime] [datetime] NULL CONSTRAINT [DF_R_Admin_ALastTime]  DEFAULT (getdate()),
 CONSTRAINT [PK_R_Admin] PRIMARY KEY CLUSTERED 
(
	[AID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'-1表示超级管理员，超级管理员访问页面将不受任何限制。' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_Admin', @level2type=N'COLUMN', @level2name=N'GID'
GO

CREATE TABLE [dbo].[R_AdminRight](
	[AID] [int] NOT NULL,
	[PID] [int] NOT NULL,
	[BtnRightExp] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
	[ClickTimes] [int] NOT NULL,
 CONSTRAINT [PK_R_AdminRight] PRIMARY KEY CLUSTERED 
(
	[AID] ASC,
	[PID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮权限表达式' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_AdminRight', @level2type=N'COLUMN', @level2name=N'BtnRightExp'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'该菜单项被点击的次数' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_AdminRight', @level2type=N'COLUMN', @level2name=N'ClickTimes'

GO
ALTER TABLE [dbo].[R_AdminRight]  WITH CHECK ADD  CONSTRAINT [FK_R_AdminRight_R_Admin] FOREIGN KEY([AID])
REFERENCES [dbo].[R_Admin] ([AID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[R_AdminRight]  WITH CHECK ADD  CONSTRAINT [FK_R_AdminRight_R_PageInfo] FOREIGN KEY([PID])
REFERENCES [dbo].[R_PageInfo] ([PID])
ON UPDATE CASCADE
ON DELETE CASCADE

GO


CREATE TABLE [dbo].[R_Group](
	[GID] [int] IDENTITY(1,1) NOT NULL,
	[GName] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_R_Group] PRIMARY KEY CLUSTERED 
(
	[GID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[R_GroupRight](
	[GID] [int] NOT NULL,
	[PID] [int] NOT NULL,
	[BtnRightExp] [nvarchar](20) COLLATE Chinese_PRC_CI_AS NOT NULL,
 CONSTRAINT [PK_R_GroupRight] PRIMARY KEY CLUSTERED 
(
	[GID] ASC,
	[PID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'按钮权限表达式' ,@level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'R_GroupRight', @level2type=N'COLUMN', @level2name=N'BtnRightExp'

GO
ALTER TABLE [dbo].[R_GroupRight]  WITH CHECK ADD  CONSTRAINT [FK_R_GroupRight_R_Group] FOREIGN KEY([GID])
REFERENCES [dbo].[R_Group] ([GID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[R_GroupRight]  WITH CHECK ADD  CONSTRAINT [FK_R_GroupRight_R_PageInfo] FOREIGN KEY([PID])
REFERENCES [dbo].[R_PageInfo] ([PID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO


CREATE TABLE [dbo].[R_PageParent](
	[RID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[PID] [int] NOT NULL,
 CONSTRAINT [PK_R_PageParent] PRIMARY KEY CLUSTERED 
(
	[RID] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[R_PageParent]  WITH CHECK ADD  CONSTRAINT [FK_R_PageParent_R_PageInfo] FOREIGN KEY([ParentID])
REFERENCES [dbo].[R_PageInfo] ([PID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO






-- =============================================
-- 检查管理员是否有目录的访问权限，同时返回对该页面按钮的访问权限表达式
-- 当有权限访问页面时，点击次数+1
-- =============================================
CREATE PROCEDURE [dbo].[p_IsInRoles]
	@aid int,   --用户ID
	@pageId int,  --页面ID -- 不确定是否是隐藏页面
	@updateClickTime bit, --是否更新点击量
	@superAdminRole nvarchar(20), --当用户是是超级管理员时的权限
	@btnRights nvarchar(20) output  --按钮权限
AS 
BEGIN
	if exists(select top 1 1 from R_Admin where AID=@aid and Gid=-1)begin
		set @btnRights = @superAdminRole
		return 1
	end
			

	select @btnRights=BtnRightExp from r_adminright where AID=@aid and PID=@pageId
	if not @btnRights is null  BEGIN --如果能找到这个权限，说明是二级
		if @updateClickTime=1
			update R_AdminRight set ClickTimes=ClickTimes+1 where AID=@aid and PID=@pageId 
		RETURN 1
	END
 
	--如果能运行到这里，表示这个页面是隐藏页
	select top 1 @btnRights=BtnRightExp from r_adminright where AID=@aid and PID in( 
		SELECT ParentID FROM R_PageParent WHERE PID = @pageId
	)

	if not @btnRights is null  BEGIN --如果能找到这个权限，说明是隐藏页
		RETURN 1 --有权限
	END

	return 0 -- 没权限
 
	if @@error>0 begin 
		return 0	
	end else begin 
		return 1	
	end
end
GO
/****** 对象:  StoredProcedure [dbo].[p_AddUpdateAdminRight]    脚本日期: 08/09/2010 15:56:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- 增加或者更新管理员权限
-- 以管理员ID、页面ID联合做主键
Create proc [dbo].[p_AddUpdateAdminRight]
@aid int,
@pid int,
@btnRightExp nvarchar(20),
@updateWhenExists bit
as
begin
	if exists(select 1 from R_AdminRight where AID=@aid and PID=@pid) begin 
		if @updateWhenExists = 1
			update R_AdminRight set BtnRightExp=@btnRightExp where AID=@aid and PID=@pid 
	end
	else
		begin
			insert into R_AdminRight(AID,PID,BtnRightExp,ClickTimes)values (@aid,@pid,@btnRightExp,0)
		end
	if @@error>0 begin return 0 end
	else begin return 1 end
end
GO



CREATE PROC [dbo].[P_UPDATE_AdminGroup] (
@adminID int,	--管理员ID
@newGroupID int,  --新的组ID
@updateRights int --是否更新权限。  0 不调整权限  1 调整为新组的权限 2 将新组的权限追加到现有权限中。
)as
begin
	UPDATE r_admin SET GID=@newGroupID WHERE AID=@adminID 
	if @updateRights= 0 
	  RETURN 1
	else if @updateRights=1 begin --调整为新组的权限
	  delete from R_AdminRight where AID=@adminID
	  INSERT INTO R_AdminRight(AID,PID,BtnRightExp,ClickTimes)
		SELECT @adminID,PID,BtnRightExp,0 FROM R_GroupRight WHERE GID=@newGroupID
	  RETURN 1
	end else if @updateRights=2 begin --将新组的权限追加到现有权限中。
	  INSERT INTO R_AdminRight(AID,PID,BtnRightExp,ClickTimes)
		SELECT @adminID,PID,BtnRightExp,0 FROM R_GroupRight WHERE GID=@newGroupID AND PID NOT IN(
		  SELECT PID FROM R_AdminRight WHERE  AID=@adminID
		)
	  RETURN 1
	end

	RETURN 0
end
GO


--删除用户
--只有超级管理员可以删除用户
-- 返回 -1 表示 用户不是管理员  1表示删除成功  0表示删除失败
Create proc [dbo].[p_DeleteAdmin]
@aid int,
@delId int
as
begin
	declare @gid int
	select @gid=GID from r_admin where AID=@AID
	if @gid=-1 begin 
		delete from R_Admin where AID=@delId
		delete from R_AdminRight where AID=@delId
		return 1
	end

	return -1
	
if @@error>0  
		return 0  
end
GO


-----增加管理员
-----如果用户组ID大于0，将用户组的预设权限复制给该用户
Create proc [dbo].[p_AddAdmin]
@AName nvarchar(20),
@APwd nvarchar(32),
@ANickName nvarchar(20),
@Email nvarchar(50),
@GID int
as
begin
	declare @newIden int

	if exists(select top 1 1 from R_Admin where AName=@AName)
		return 0

	begin tran
		insert into R_Admin(AName,APwd,ANickName,Email,GID)values (@AName,@APwd,@ANickName,@Email,@GID)
		select @newIden = @@IDENTITY
		if @GID>0 begin
			INSERT INTO R_AdminRight(AID,PID,BtnRightExp,ClickTimes) SELECT @newIden,PID,BtnRightExp,0 FROM R_GroupRight WHERE GID=@GID
		end 
	if(@@error<>0) begin 
		rollback
		return 0;
	end
	else begin
		commit
		return @newIden;
	end
end
GO



CREATE proc [dbo].[p_AddUpdateGroupRight]
@gid int,
@pid int,
@btnRightExp nvarchar(20),
@updateWhenExists bit
as
begin
	if exists(select 1 from R_GroupRight where GID=@gid and PID=@pid) begin
		if @updateWhenExists = 1
			update R_GroupRight set BtnRightExp=@btnRightExp where GID=@gid and PID=@pid 
	end else begin
		insert into R_GroupRight(GID,PID,BtnRightExp)values (@gid,@pid,@btnRightExp)
	end
	if(@@error>0) begin return 0 end
	else begin return 1 end
end
GO

/*
!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!这个是超级管理员的帐号!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
*/
insert into R_Admin(AName,APwd,ANickName,Email,GID)
values('zum','','庄铭','',-1)


declare @tmpId1 int,@tmpId2 int;

insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('权限管理','',0,1,0)
insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('管理员列表','Role/AdminList.aspx',1,0,1)
set @tmpId1=@@identity;
insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('页面管理','Role/PageList.aspx',1,0,1)
insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('权限分配','Role/RoleManage.aspx',1,0,1)
insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('用户组列表','Role/GroupList.aspx',1,0,1)
insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('管理首页','',0,-1,0)

insert into R_PageInfo(PName,PUrl,IsUrl,[Queue],ParentID)
values('快速添加管理员','Role/FastAddAdmin.aspx',1,0,-1)
set @tmpId2=@@identity;

insert into R_PageParent(PID,ParentID)
values(@tmpId2,@tmpId1)
