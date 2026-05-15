# CryptoTool 构建说明

## 自动化构建和打包

本项目支持通过 GitHub Actions 自动构建和打包，支持以下格式：
- `.exe` 可执行文件（自包含和框架依赖版本）
- `.msi` Windows 安装包
- `.zip` 压缩包

### 版本管理

版本号的获取优先级如下：
1. **Git标签版本**：当推送带有 `v*.*.*` 格式的标签时（如 `v1.2.0`），会自动使用标签版本
2. **项目文件版本**：从 `CryptoTool.Win/CryptoTool.Win.csproj` 中的 `<Version>` 属性读取

### 自动发布流程

#### 开发版本构建
每次推送到 `master` 分支或创建 Pull Request 时，会自动构建所有配置的包，但不会创建 GitHub Release。

#### 正式版本发布
1. 更新项目版本号（可选，也可以直接使用 Git 标签版本）：
   ```xml
   <!-- 在 CryptoTool.Win/CryptoTool.Win.csproj 中 -->
   <Version>1.2.0</Version>
   <AssemblyVersion>1.2.0.0</AssemblyVersion>
   <FileVersion>1.2.0.0</FileVersion>
   ```

2. 创建并推送标签：
   ```bash
   git tag v1.2.0
   git push origin v1.2.0
   ```

3. GitHub Actions 会自动：
   - 构建所有包格式
   - 创建 GitHub Release
   - 上传所有构建产物

### 构建产物说明

每次构建会生成以下文件：

#### 可执行文件
- `CryptoTool-v{version}-win-x64-SelfContained.exe` - 自包含版本，无需安装 .NET 运行时
- `CryptoTool-v{version}-win-x64-FrameworkDependent.exe` - 需要 .NET 8 运行时

#### 安装包
- `CryptoTool-v{version}-win-x64-Setup.msi` - Windows 安装包，自动创建桌面和开始菜单快捷方式

#### 压缩包
- `CryptoTool-v{version}-SelfContained-Release.zip` - 自包含版本的完整文件
- `CryptoTool-v{version}-FrameworkDependent-Release.zip` - 框架依赖版本的完整文件

## 本地构建

### 前置条件
- .NET 8 SDK
- WiX Toolset v5（用于构建 MSI）

### 安装 WiX Toolset
```bash
dotnet tool install --global wix --version 5.0.0
wix extension add WixToolset.UI.wixext
```

### 构建方法

#### 方法1：使用批处理文件（推荐）
双击运行 `build-msi.bat` 或在命令行中执行：
```bash
build-msi.bat
```

#### 方法2：使用 PowerShell 脚本
```powershell
# 使用项目文件中的版本号
./build-msi.ps1

# 指定版本号
./build-msi.ps1 -Version "1.2.0"

# 指定配置
./build-msi.ps1 -Configuration "Debug"
```

#### 方法3：手动构建
```bash
# 构建解决方案
dotnet build CryptoTool.sln --configuration Release

# 发布应用
dotnet publish CryptoTool.Win/CryptoTool.Win.csproj --configuration Release --runtime win-x64 --self-contained true --output ./publish/CryptoTool.Win-SelfContained -p:PublishSingleFile=true

# 构建 MSI（需要先创建 WiX 配置文件）
wix build installer/CryptoTool.wxs -o CryptoTool-Setup.msi
```

## 配置说明

### GitHub Actions 配置
工作流配置位于 `.github/workflows/dotnet-desktop.yml`，包含以下主要步骤：
- 版本检测（Git 标签 > 项目文件版本）
- 多配置构建（Debug/Release）
- MSI 包创建
- 自动发布到 GitHub Releases

### MSI 安装包配置
- 安装位置：`%ProgramFiles%\CryptoTool\`
- 自动创建桌面快捷方式
- 自动创建开始菜单快捷方式
- 支持升级和卸载

### 项目配置
关键的项目配置位于 `CryptoTool.Win/CryptoTool.Win.csproj`：
- 版本信息
- 应用程序图标
- 产品描述
- 公司信息

### 自动化测试
- 主测试入口为 `CryptoTool.UnitTests/CryptoTool.UnitTests.csproj`
- 本地推荐执行：
  ```bash
  dotnet test CryptoTool.UnitTests/CryptoTool.UnitTests.csproj
  ```

## 故障排除

### WiX 安装问题
如果遇到 WiX 相关错误：
```bash
# 卸载并重新安装
dotnet tool uninstall --global wix
dotnet tool install --global wix --version 5.0.0
wix extension add WixToolset.UI.wixext
```

### 构建失败
1. 确保所有依赖项已正确安装
2. 检查项目文件中的版本号格式
3. 确保发布目录存在且可写
4. 检查 WiX 配置文件语法

### GitHub Actions 失败
1. 检查项目文件中的版本号格式
2. 确保标签格式为 `v*.*.*`
3. 检查工作流权限设置
4. 查看 Actions 日志获取详细错误信息
