using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using Org.BouncyCastle.Crypto.Parameters;

namespace CryptoTool.Win;

public partial class EcdsaTabControl
{
    #region 密钥存储标准常量

    private const string PrivateKeyStandardSec1 = "SEC1/RFC 5915（长编码 / specifiedCurve）";
    private const string PrivateKeyStandardPkcs8 = "PKCS#8 / RFC 5958（短编码 / namedCurve）";

    private const string PublicKeyStandardNamedCurve = "RFC 5480/namedCurve";
    private const string PublicKeyStandardSpecifiedCurve = "RFC 5480/specifiedCurve";

    #endregion

    #region 密钥存储标准初始化

    /// <summary>
    /// 初始化 ECDSA 上方面板的密钥存储标准下拉框选项
    /// </summary>
    private void InitializeKeyStandards()
    {
        comboPrivateKeyStandard.Items.Clear();
        comboPrivateKeyStandard.Items.AddRange([PrivateKeyStandardPkcs8, PrivateKeyStandardSec1]);
        comboPrivateKeyStandard.SelectedIndex = 0;

        comboPublicKeyStandard.Items.Clear();
        comboPublicKeyStandard.Items.AddRange([PublicKeyStandardNamedCurve, PublicKeyStandardSpecifiedCurve]);
        comboPublicKeyStandard.SelectedIndex = 0;
    }

    #endregion

    #region 密钥导出（按存储标准）

    /// <summary>
    /// 根据下拉框选项将 EC 私钥导出为 SEC1 或 PKCS#8
    /// </summary>
    private string ExportPrivateKeyByStandard(ECPrivateKeyParameters priv)
    {
        string standard = comboPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardSec1;
        return ExportPrivateKeyByStandard(priv, standard);
    }

    private static string ExportPrivateKeyByStandard(ECPrivateKeyParameters priv, string standard)
    {
        if (standard == PrivateKeyStandardPkcs8)
            return EcdsaKeyHelper.ExportPrivateKeyPemPkcs8(priv);

        // SEC1/RFC 5915: 强制使用显式参数 (specifiedCurve)，避免 namedCurve 私钥被输出为短编码
        var explicitParams = new ECDomainParameters(
            priv.Parameters.Curve, priv.Parameters.G, priv.Parameters.N, priv.Parameters.H, priv.Parameters.GetSeed());
        var explicitPriv = new ECPrivateKeyParameters(priv.D, explicitParams);
        return EcdsaKeyHelper.ExportPrivateKeyPem(explicitPriv);
    }

    /// <summary>
    /// 根据下拉框选项将 EC 公钥导出为 namedCurve 或 specifiedCurve
    /// </summary>
    private string ExportPublicKeyByStandard(ECPublicKeyParameters pub)
    {
        string standard = comboPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardSpecifiedCurve;
        return ExportPublicKeyByStandard(pub, standard);
    }

    private static string ExportPublicKeyByStandard(ECPublicKeyParameters pub, string standard)
    {
        if (standard == PublicKeyStandardNamedCurve)
            return EcdsaKeyHelper.ExportPublicKeyPemNamedCurve(pub);

        // specifiedCurve: 强制使用显式参数，避免 namedCurve 导入后又被输出为 OID 格式
        var explicitParams = new ECDomainParameters(
            pub.Parameters.Curve, pub.Parameters.G, pub.Parameters.N, pub.Parameters.H, pub.Parameters.GetSeed());
        var explicitPub = new ECPublicKeyParameters(pub.Q, explicitParams);
        return EcdsaKeyHelper.ExportPublicKeyPem(explicitPub);
    }

    #endregion

    #region ECDSA 密钥存储标准转换按钮

    private void BtnConvertPrivateKeyStandard_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_privateKeyPem))
            {
                MessageBox.Show("当前没有私钥内容可转换，请先生成或导入私钥。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string pem = ConvertDisplayToPem(_privateKeyPem, true);
            ECPrivateKeyParameters priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
            _privateKeyPem = ExportPrivateKeyByStandard(priv);
            textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, GetCurrentOutputFormat());

            string standard = comboPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardSec1;
            AppendValidationResult($"私钥已转换为 {standard}", Color.Gray);
            SetStatus($"私钥存储标准转换完成 - {standard}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"转换私钥存储标准失败：{ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("私钥存储标准转换失败");
        }
    }

    private void BtnConvertPublicKeyStandard_Click(object? sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_publicKeyPem))
            {
                MessageBox.Show("当前没有公钥内容可转换，请先生成或导入公钥。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string pem = ConvertDisplayToPem(_publicKeyPem, false);
            ECPublicKeyParameters pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
            _publicKeyPem = ExportPublicKeyByStandard(pub);
            textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, GetCurrentOutputFormat());

            string standard = comboPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardSpecifiedCurve;
            AppendValidationResult($"公钥已转换为 {standard}", Color.Gray);
            SetStatus($"公钥存储标准转换完成 - {standard}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"转换公钥存储标准失败：{ex.Message}", "错误",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            SetStatus("公钥存储标准转换失败");
        }
    }

    #endregion

    #region ECDH 密钥存储标准转换按钮

    private void BtnConvertEcdhPrivateKeyStandard_Click(object? sender, EventArgs e)
    {
        try
        {
            string standard = comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardPkcs8;
            int convertedCount = 0;
            if (ConvertEcdhPrivateKey(textEcdhAlicePrivate, standard)) convertedCount++;
            if (ConvertEcdhPrivateKey(textEcdhBobPrivate, standard)) convertedCount++;

            if (convertedCount == 0)
            {
                MessageBox.Show("当前没有 Alice 或 Bob 的私钥内容可转换，请先生成或导入私钥。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AppendValidationResult($"✅ {convertedCount} 组私钥已转换为 {standard}", Color.Gray);
            SetStatus($"Alice/Bob 私钥存储标准转换完成 - {standard}");
        }
        catch (Exception ex)
        {
            AppendValidationResult($"❌ ECDH 私钥标准转换失败: {ex.Message}", Color.Red);
            SetStatus("ECDH 私钥标准转换失败");
        }
    }

    private static bool ConvertEcdhPrivateKey(TextBox textBox, string standard)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text))
            return false;

        string pem = ConvertDisplayToPem(textBox.Text.Trim(), true);
        var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
        string converted = ExportPrivateKeyByStandard(priv, standard);
        textBox.Text = FormatKeyForDisplay(converted, UIOutputFormat.PEM);
        return true;
    }

    private void BtnConvertEcdhPublicKeyStandard_Click(object? sender, EventArgs e)
    {
        try
        {
            string standard = comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve;
            int convertedCount = 0;
            if (ConvertEcdhPublicKey(textEcdhAlicePublic, standard)) convertedCount++;
            if (ConvertEcdhPublicKey(textEcdhBobPublic, standard)) convertedCount++;

            if (convertedCount == 0)
            {
                MessageBox.Show("当前没有 Alice 或 Bob 的公钥内容可转换，请先生成或导入公钥。", "提示",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            AppendValidationResult($"✅ {convertedCount} 组公钥已转换为 {standard}", Color.Gray);
            SetStatus($"Alice/Bob 公钥存储标准转换完成 - {standard}");
        }
        catch (Exception ex)
        {
            AppendValidationResult($"❌ ECDH 公钥标准转换失败: {ex.Message}", Color.Red);
            SetStatus("ECDH 公钥标准转换失败");
        }
    }

    private static bool ConvertEcdhPublicKey(TextBox textBox, string standard)
    {
        if (string.IsNullOrWhiteSpace(textBox.Text))
            return false;

        string pem = ConvertDisplayToPem(textBox.Text.Trim(), false);
        var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
        string converted = ExportPublicKeyByStandard(pub, standard);
        textBox.Text = FormatKeyForDisplay(converted, UIOutputFormat.PEM);
        return true;
    }

    private void ComboEcdhPrivateKeyStandard_SelectedIndexChanged(object? sender, EventArgs e)
    {
        try
        {
            string standard = comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardPkcs8;
            ConvertEcdhPrivateKey(textEcdhAlicePrivate, standard);
            ConvertEcdhPrivateKey(textEcdhBobPrivate, standard);
        }
        catch
        {
            // 下拉框选项变化时仅静默转换已存在的密钥，避免弹窗干扰操作
        }
    }

    private void ComboEcdhPublicKeyStandard_SelectedIndexChanged(object? sender, EventArgs e)
    {
        try
        {
            string standard = comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve;
            ConvertEcdhPublicKey(textEcdhAlicePublic, standard);
            ConvertEcdhPublicKey(textEcdhBobPublic, standard);
        }
        catch
        {
            // 下拉框选项变化时仅静默转换已存在的密钥，避免弹窗干扰操作
        }
    }

    #endregion

    #region ECDH 面板标准选择行与曲线选择行创建

    /// <summary>
    /// 创建 ECDH 面板的私钥/公钥存储标准转换行
    /// </summary>
    private FlowLayoutPanel CreateEcdhPrivateStandardRow()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(50, 0, 0, 0),
            Padding = new Padding(0, 8, 0, 9),
            WrapContents = false
        };
        var lbl = new Label
        {
            Text = "私钥存储标准：",
            AutoSize = false,
            Size = new Size(130, 32),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 3, 4, 3)
        };
        comboEcdhPrivateKeyStandard = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Size = new Size(300, 32),
            Margin = new Padding(0, 3, 4, 3)
        };
        comboEcdhPrivateKeyStandard.Items.AddRange([PrivateKeyStandardPkcs8, PrivateKeyStandardSec1]);
        comboEcdhPrivateKeyStandard.SelectedIndex = 0;
        btnConvertEcdhPrivateKeyStandard = new Button
        {
            Text = "转换",
            Size = new Size(60, 32),
            Margin = new Padding(0, 3, 0, 3)
        };
        btnConvertEcdhPrivateKeyStandard.Click += BtnConvertEcdhPrivateKeyStandard_Click;
        comboEcdhPrivateKeyStandard.SelectedIndexChanged += ComboEcdhPrivateKeyStandard_SelectedIndexChanged;
        panel.Controls.Add(lbl);
        panel.Controls.Add(comboEcdhPrivateKeyStandard);
        panel.Controls.Add(btnConvertEcdhPrivateKeyStandard);
        return panel;
    }

    /// <summary>
    /// 创建 ECDH 面板的公钥存储标准转换行
    /// </summary>
    private FlowLayoutPanel CreateEcdhPublicStandardRow()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            Margin = new Padding(50, 0, 0, 0),
            Padding = new Padding(0, 8, 0, 9),
            WrapContents = false
        };
        var lbl = new Label
        {
            Text = "公钥存储标准：",
            AutoSize = false,
            Size = new Size(130, 32),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 3, 4, 3)
        };
        comboEcdhPublicKeyStandard = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Size = new Size(300, 32),
            Margin = new Padding(0, 3, 4, 3)
        };
        comboEcdhPublicKeyStandard.Items.AddRange([PublicKeyStandardNamedCurve, PublicKeyStandardSpecifiedCurve]);
        comboEcdhPublicKeyStandard.SelectedIndex = 0;
        btnConvertEcdhPublicKeyStandard = new Button
        {
            Text = "转换",
            Size = new Size(60, 32),
            Margin = new Padding(0, 3, 0, 3)
        };
        btnConvertEcdhPublicKeyStandard.Click += BtnConvertEcdhPublicKeyStandard_Click;
        comboEcdhPublicKeyStandard.SelectedIndexChanged += ComboEcdhPublicKeyStandard_SelectedIndexChanged;
        panel.Controls.Add(lbl);
        panel.Controls.Add(comboEcdhPublicKeyStandard);
        panel.Controls.Add(btnConvertEcdhPublicKeyStandard);
        return panel;
    }

    /// <summary>
    /// 创建 ECDH 面板的椭圆曲线选择行（类别 → 曲线名）
    /// 注意：comboEcdhCategory.Items 和 SelectedIndexChanged 事件由 InitializeEcdhCurveList() 设置
    /// </summary>
    private FlowLayoutPanel CreateEcdhCurveRow()
    {
        var panel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            Margin = new Padding(50, 0, 0, 0),
            Padding = new Padding(0, 8, 0, 9),
            WrapContents = false
        };
        var lblCurve = new Label
        {
            Text = "椭圆曲线：",
            AutoSize = false,
            Size = new Size(200, 32),
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(0, 3, 8, 3)
        };
        comboEcdhCategory = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Size = new Size(183, 32),
            Margin = new Padding(0, 3, 4, 3)
        };
        var lblArrow = new Label
        {
            Text = "→",
            AutoSize = false,
            Size = new Size(36, 32),
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(4, 3, 4, 3)
        };
        comboEcdhCurve = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Value",
            ValueMember = "Key",
            Size = new Size(330, 32),
            Margin = new Padding(0, 3, 8, 3)
        };
        panel.Controls.Add(lblCurve);
        panel.Controls.Add(comboEcdhCategory);
        panel.Controls.Add(lblArrow);
        panel.Controls.Add(comboEcdhCurve);
        return panel;
    }

    #endregion

    #region 上半部分：ECDSA 面板曲线与标准 UI（设计器拆分）

    /// <summary>
    /// 上半部分：初始化 ECDSA 面板的曲线类别与密钥存储标准下拉框
    /// 原位于 EcdsaTabControl.InitializeDefaults，拆出便于管理
    /// </summary>
    private void InitializeEcdsaCurveAndStandardControls()
    {
        _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

        comboCategory.DisplayMember = "Text";
        comboCategory.ValueMember = "Value";
        comboCategory.Items.Clear();
        foreach (var cat in _allCurveData)
        {
            comboCategory.Items.Add(new
            {
                Text = $"{cat.Value.Icon} {cat.Key}",
                Value = cat.Key
            });
        }

        if (comboCategory.Items.Count > 0)
            comboCategory.SelectedIndex = 0;

        InitializeKeyStandards();
    }

    private void ComboCategory_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender == null || comboCategory.SelectedItem == null)
            return;

        dynamic selectedItem = comboCategory.SelectedItem;
        string categoryKey = selectedItem.Value;

        if (!_allCurveData.TryGetValue(categoryKey, out var categoryData))
            return;

        comboCurve.Items.Clear();
        foreach (var c in categoryData.Curves)
            comboCurve.Items.Add(c);

        if (comboCurve.Items.Count > 0)
            comboCurve.SelectedIndex = 0;
    }

    private string GetSelectedCurve()
    {
        if (comboCurve.SelectedItem is KeyValuePair<string, string> sel && !string.IsNullOrEmpty(sel.Key))
            return sel.Key;
        return "prime256v1";
    }

    private void LabelCurveHeader_Click(object? sender, EventArgs e)
    {
    }

    private void LabelCurve_Click(object? sender, EventArgs e)
    {
    }

    /// <summary>
    /// 上半部分：初始化 ECDSA 面板右侧的曲线与标准选择行
    /// 原位于 EcdsaTabControl.Designer.cs InitializeComponent，拆出便于管理
    /// </summary>
    private void InitializeEcdsaCurveAndStandardRows()
    {
        // panelPrivateKeyStandardRow
        panelPrivateKeyStandardRow.AutoSize = true;
        panelPrivateKeyStandardRow.Controls.Add(labelPrivateKeyStandard);
        panelPrivateKeyStandardRow.Controls.Add(comboPrivateKeyStandard);
        panelPrivateKeyStandardRow.Controls.Add(btnConvertPrivateKeyStandard);
        panelPrivateKeyStandardRow.Location = new Point(3, 93);
        panelPrivateKeyStandardRow.Name = "panelPrivateKeyStandardRow";
        panelPrivateKeyStandardRow.Padding = new Padding(6, 0, 6, 0);
        panelPrivateKeyStandardRow.Size = new Size(450, 38);
        panelPrivateKeyStandardRow.TabIndex = 7;

        // labelPrivateKeyStandard
        labelPrivateKeyStandard.AutoSize = true;
        labelPrivateKeyStandard.Location = new Point(9, 7);
        labelPrivateKeyStandard.Margin = new Padding(3, 7, 3, 3);
        labelPrivateKeyStandard.Name = "labelPrivateKeyStandard";
        labelPrivateKeyStandard.Size = new Size(118, 24);
        labelPrivateKeyStandard.TabIndex = 0;
        labelPrivateKeyStandard.Text = "私钥存储标准：";

        // comboPrivateKeyStandard
        comboPrivateKeyStandard.DropDownStyle = ComboBoxStyle.DropDownList;
        comboPrivateKeyStandard.FormattingEnabled = true;
        comboPrivateKeyStandard.Location = new Point(133, 3);
        comboPrivateKeyStandard.Margin = new Padding(0, 3, 8, 3);
        comboPrivateKeyStandard.Name = "comboPrivateKeyStandard";
        comboPrivateKeyStandard.Size = new Size(420, 32);
        comboPrivateKeyStandard.TabIndex = 1;

        // btnConvertPrivateKeyStandard
        btnConvertPrivateKeyStandard.AutoSize = true;
        btnConvertPrivateKeyStandard.Location = new Point(361, 3);
        btnConvertPrivateKeyStandard.Margin = new Padding(0, 3, 4, 3);
        btnConvertPrivateKeyStandard.MinimumSize = new Size(80, 26);
        btnConvertPrivateKeyStandard.Name = "btnConvertPrivateKeyStandard";
        btnConvertPrivateKeyStandard.Size = new Size(80, 34);
        btnConvertPrivateKeyStandard.TabIndex = 2;
        btnConvertPrivateKeyStandard.Text = "转换";
        btnConvertPrivateKeyStandard.Click += BtnConvertPrivateKeyStandard_Click;

        // panelPublicKeyStandardRow
        panelPublicKeyStandardRow.AutoSize = true;
        panelPublicKeyStandardRow.Controls.Add(labelPublicKeyStandard);
        panelPublicKeyStandardRow.Controls.Add(comboPublicKeyStandard);
        panelPublicKeyStandardRow.Controls.Add(btnConvertPublicKeyStandard);
        panelPublicKeyStandardRow.Location = new Point(3, 93);
        panelPublicKeyStandardRow.Name = "panelPublicKeyStandardRow";
        panelPublicKeyStandardRow.Padding = new Padding(6, 0, 6, 0);
        panelPublicKeyStandardRow.Size = new Size(450, 38);
        panelPublicKeyStandardRow.TabIndex = 8;

        // labelPublicKeyStandard
        labelPublicKeyStandard.AutoSize = true;
        labelPublicKeyStandard.Location = new Point(9, 7);
        labelPublicKeyStandard.Margin = new Padding(3, 7, 3, 3);
        labelPublicKeyStandard.Name = "labelPublicKeyStandard";
        labelPublicKeyStandard.Size = new Size(118, 24);
        labelPublicKeyStandard.TabIndex = 0;
        labelPublicKeyStandard.Text = "公钥存储标准：";

        // comboPublicKeyStandard
        comboPublicKeyStandard.DropDownStyle = ComboBoxStyle.DropDownList;
        comboPublicKeyStandard.FormattingEnabled = true;
        comboPublicKeyStandard.Location = new Point(133, 3);
        comboPublicKeyStandard.Margin = new Padding(0, 3, 8, 3);
        comboPublicKeyStandard.Name = "comboPublicKeyStandard";
        comboPublicKeyStandard.Size = new Size(420, 32);
        comboPublicKeyStandard.TabIndex = 1;

        // btnConvertPublicKeyStandard
        btnConvertPublicKeyStandard.AutoSize = true;
        btnConvertPublicKeyStandard.Location = new Point(361, 3);
        btnConvertPublicKeyStandard.Margin = new Padding(0, 3, 4, 3);
        btnConvertPublicKeyStandard.MinimumSize = new Size(80, 26);
        btnConvertPublicKeyStandard.Name = "btnConvertPublicKeyStandard";
        btnConvertPublicKeyStandard.Size = new Size(80, 34);
        btnConvertPublicKeyStandard.TabIndex = 2;
        btnConvertPublicKeyStandard.Text = "转换";
        btnConvertPublicKeyStandard.Click += BtnConvertPublicKeyStandard_Click;

        // panelCurveContainer
        panelCurveContainer.Controls.Add(panelCurveRow);
        panelCurveContainer.Location = new Point(3, 93);
        panelCurveContainer.Name = "panelCurveContainer";
        panelCurveContainer.Padding = new Padding(6, 0, 6, 0);
        panelCurveContainer.Size = new Size(920, 38);
        panelCurveContainer.TabIndex = 2;

        // panelCurveRow
        panelCurveRow.Controls.Add(labelCurve);
        panelCurveRow.Controls.Add(comboCategory);
        panelCurveRow.Controls.Add(lblArrow);
        panelCurveRow.Controls.Add(comboCurve);
        panelCurveRow.Location = new Point(6, 0);
        panelCurveRow.Name = "panelCurveRow";
        panelCurveRow.Padding = new Padding(0, 2, 0, 2);
        panelCurveRow.Size = new Size(900, 38);
        panelCurveRow.TabIndex = 1;
        panelCurveRow.WrapContents = false;

        // labelCurve
        labelCurve.Location = new Point(34, 5);
        labelCurve.Margin = new Padding(34, 3, 2, 3);
        labelCurve.Name = "labelCurve";
        labelCurve.Size = new Size(200, 32);
        labelCurve.TabIndex = 0;
        labelCurve.Text = "椭圆曲线：";
        labelCurve.TextAlign = ContentAlignment.MiddleLeft;
        labelCurve.Click += LabelCurve_Click;

        // comboCategory
        comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
        comboCategory.FormattingEnabled = true;
        comboCategory.Location = new Point(136, 5);
        comboCategory.Margin = new Padding(0, 3, 4, 3);
        comboCategory.Name = "comboCategory";
        comboCategory.Size = new Size(183, 32);
        comboCategory.TabIndex = 1;
        comboCategory.SelectedIndexChanged += ComboCategory_SelectedIndexChanged;

        // lblArrow
        lblArrow.Location = new Point(327, 5);
        lblArrow.Margin = new Padding(4, 3, 4, 3);
        lblArrow.Name = "lblArrow";
        lblArrow.Padding = new Padding(4, 0, 4, 0);
        lblArrow.Size = new Size(36, 32);
        lblArrow.TabIndex = 2;
        lblArrow.Text = "→";
        lblArrow.TextAlign = ContentAlignment.MiddleCenter;

        // comboCurve
        comboCurve.DisplayMember = "Value";
        comboCurve.DropDownStyle = ComboBoxStyle.DropDownList;
        comboCurve.FormattingEnabled = true;
        comboCurve.Location = new Point(367, 5);
        comboCurve.Margin = new Padding(0, 3, 4, 3);
        comboCurve.Name = "comboCurve";
        comboCurve.Size = new Size(330, 32);
        comboCurve.TabIndex = 3;
        comboCurve.ValueMember = "Key";

        // 将三行加入右侧设置面板
        panelRightSettings.Controls.Add(panelPrivateKeyStandardRow);
        panelRightSettings.Controls.Add(panelPublicKeyStandardRow);
        panelRightSettings.Controls.Add(panelCurveContainer);
    }

    #endregion
}
