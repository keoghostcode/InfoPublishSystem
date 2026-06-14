# 信息发布系统 (InfoPublishSystem)

一个基于 ASP.NET MVC 5 开发的信息发布与管理系统，支持新闻发布、栏目管理、用户留言审核等功能。

## 功能特性

### 用户功能
- 用户注册与登录
- 浏览新闻列表（支持按栏目筛选）
- 查看新闻详情
- 发表评论与回复

### 管理员功能
- 后台管理面板
- 栏目管理（添加、编辑、删除）
- 新闻管理（发布、编辑、删除）
- 评论审核（通过/拒绝）

## 技术栈

- **框架**: ASP.NET MVC 5.2.9
- **运行时**: .NET Framework 4.8
- **ORM**: Entity Framework 6.5.2
- **数据库**: SQL Server
- **前端框架**: Bootstrap 5
- **JavaScript库**: jQuery 3.7.0
- **密码加密**: BCrypt.Net-Next 4.2.0

## 项目结构

```
InfoPublishSystem/
├── Controllers/
│   ├── AccountController.cs    # 用户账户管理
│   ├── AdminController.cs      # 后台管理
│   ├── CommentController.cs    # 评论管理
│   └── HomeController.cs       # 前台页面
├── Models/
│   ├── User.cs                 # 用户模型
│   ├── News.cs                 # 新闻模型
│   ├── Category.cs             # 栏目模型
│   ├── Comment.cs              # 评论模型
│   ├── InfoDbContext.cs        # 数据库上下文
│   └── ViewModels/             # 视图模型
├── Views/
│   ├── Account/                # 账户相关视图
│   ├── Admin/                  # 后台管理视图
│   ├── Home/                   # 前台页面视图
│   └── Shared/                 # 共享布局
├── Filters/
│   ├── AdminAuthorizeAttribute.cs  # 管理员授权
│   └── LoginAuthorizeAttribute.cs  # 登录授权
└── Scripts/
    └── Database/
        └── Init.sql            # 数据库初始化脚本
```

## 数据库设计

### 用户表 (Users)
| 字段 | 类型 | 说明 |
|------|------|------|
| UserId | INT | 主键，自增 |
| UserName | NVARCHAR(50) | 用户名，唯一 |
| Password | NVARCHAR(100) | 密码（BCrypt加密） |
| Email | NVARCHAR(100) | 邮箱 |
| Role | NVARCHAR(20) | 角色（Admin/User） |
| RegisterTime | DATETIME | 注册时间 |

### 栏目表 (Categories)
| 字段 | 类型 | 说明 |
|------|------|------|
| CategoryId | INT | 主键，自增 |
| CategoryName | NVARCHAR(50) | 栏目名称 |
| SortOrder | INT | 排序顺序 |

### 新闻表 (News)
| 字段 | 类型 | 说明 |
|------|------|------|
| NewsId | INT | 主键，自增 |
| Title | NVARCHAR(200) | 新闻标题 |
| Content | NVARCHAR(MAX) | 新闻内容 |
| CategoryId | INT | 栏目ID（外键） |
| AuthorId | INT | 作者ID（外键） |
| PublishTime | DATETIME | 发布时间 |
| IsVisible | BIT | 是否可见 |

### 评论表 (Comments)
| 字段 | 类型 | 说明 |
|------|------|------|
| CommentId | INT | 主键，自增 |
| NewsId | INT | 新闻ID（外键） |
| UserId | INT | 用户ID（外键） |
| Content | NVARCHAR(500) | 评论内容 |
| CreateTime | DATETIME | 创建时间 |
| ParentCommentId | INT | 父评论ID（支持回复） |
| IsApproved | BIT | 是否审核通过 |
| IsDeleted | BIT | 是否软删除 |

## 安装部署

### 环境要求
- Visual Studio 2017 或更高版本
- .NET Framework 4.8
- SQL Server 2012 或更高版本
- IIS Express 或 IIS

### 部署步骤

1. **克隆项目**
   ```bash
   git clone https://github.com/your-username/InfoPublishSystem.git
   cd InfoPublishSystem
   ```

2. **创建数据库**
   - 在 SQL Server 中创建新数据库
   - 执行 `Scripts/Database/Init.sql` 初始化数据库结构

3. **配置连接字符串**
   
   打开 `Web.config`，修改数据库连接字符串：
   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Data Source=.;Initial Catalog=InfoPublishSystem;Integrated Security=True" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **还原 NuGet 包**
   - 在 Visual Studio 中打开解决方案
   - 右键点击解决方案 → "还原 NuGet 包"

5. **运行项目**
   - 按 F5 或点击"启动调试"
   - 项目将在 IIS Express 中运行

### 默认管理员账号
- 用户名: `admin`
- 密码: `admin123`

## 使用说明

### 前台功能
- 访问首页浏览新闻列表
- 点击栏目筛选新闻
- 点击新闻标题查看详情
- 登录后可发表评论

### 后台管理
- 访问 `/Admin` 进入后台
- 使用管理员账号登录
- 管理栏目、新闻和评论审核

## 安全特性

- 密码使用 BCrypt 加密存储
- 使用防伪令牌 (AntiForgeryToken) 防止 CSRF 攻击
- 管理员页面使用授权过滤器保护
- 用户输入验证

## 许可证

本项目仅供学习交流使用。