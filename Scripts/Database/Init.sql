-- 信息发布系统数据库初始化脚本

-- 1. 用户表
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Role NVARCHAR(20) NOT NULL DEFAULT 'User', -- Admin / User
    RegisterTime DATETIME DEFAULT GETDATE()
);

-- 2. 栏目表
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL,
    SortOrder INT DEFAULT 0
);

-- 3. 新闻/信息表
CREATE TABLE News (
    NewsId INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX),
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    AuthorId INT FOREIGN KEY REFERENCES Users(UserId),
    PublishTime DATETIME DEFAULT GETDATE(),
    IsVisible BIT DEFAULT 1
);

-- 4. 留言表（支持级联 + 审核）
CREATE TABLE Comments (
    CommentId INT PRIMARY KEY IDENTITY(1,1),
    NewsId INT FOREIGN KEY REFERENCES News(NewsId),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    Content NVARCHAR(500) NOT NULL,
    CreateTime DATETIME DEFAULT GETDATE(),
    ParentCommentId INT NULL FOREIGN KEY REFERENCES Comments(CommentId),
    IsApproved BIT DEFAULT 0,      -- 0未审核 1通过
    IsDeleted BIT DEFAULT 0        -- 软删除
);

-- 插入管理员账号（密码为 admin123 的 BCrypt 哈希值）
INSERT INTO Users (UserName, Password, Email, Role) 
VALUES ('admin', '$2a$12$EixZaYbB.rK4fl8x2q7Meu6Q6D2V5fF5Q5Q5Q5Q5Q5Q5Q5Q5Q5Q', 'admin@example.com', 'Admin');

-- 插入初始栏目
INSERT INTO Categories (CategoryName, SortOrder) VALUES ('新闻动态', 1);
INSERT INTO Categories (CategoryName, SortOrder) VALUES ('通知公告', 2);
INSERT INTO Categories (CategoryName, SortOrder) VALUES ('行业资讯', 3);
INSERT INTO Categories (CategoryName, SortOrder) VALUES ('政策法规', 4);
