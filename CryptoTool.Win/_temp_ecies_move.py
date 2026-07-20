import sys

# ===================== Process EcdsaTabControl.cs =====================
with open(r'E:\Users\Documents\GitHub\CryptoTool\CryptoTool.Win\EcdsaTabControl.cs', 'r', encoding='utf-8') as f:
    content = f.read()

# 1. Delete fields
content = content.replace(
    '        private byte[]? _derivedEncKey = null;\n        private byte[]? _lastEncIV = null;\n',
    '\n'
)

# 2. Modify InitializeDefaults
old_defaults = '''            if (comboEncMode.Items.Count > 0)
                comboEncMode.SelectedIndex = 0;
            if (comboEncInputFormat.Items.Count > 0)
                comboEncInputFormat.SelectedIndex = 0;
            if (comboEncOutputFormat.Items.Count > 0)
                comboEncOutputFormat.SelectedIndex = 0;

            InitializeEncryptCurveList();'''
new_defaults = '            InitializeEncryptDefaults();'
content = content.replace(old_defaults, new_defaults)

# 3. Delete ComboEncCurveCategory_SelectedIndexChanged
old_combo = '''        private void ComboEncCurveCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender == null || comboEncCurveCategory.SelectedItem == null)
                return;

            dynamic selectedItem = comboEncCurveCategory.SelectedItem;
            string categoryKey = selectedItem.Value;

            if (!_allCurveData.TryGetValue(categoryKey, out var categoryData))
                return;

            comboEncCurve.Items.Clear();
            foreach (var c in categoryData.Curves)
                comboEncCurve.Items.Add(c);

            if (comboEncCurve.Items.Count > 0)
                comboEncCurve.SelectedIndex = 0;
        }
        #endregion'''
new_combo = '        #endregion'
content = content.replace(old_combo, new_combo)

# 4. Delete #region 加密与解密逻辑 (with nested regions)
start = content.find('        #region 加密与解密逻辑')
if start != -1:
    pos = start
    depth = 0
    while True:
        region_start = content.find('\n        #region ', pos)
        region_end = content.find('\n        #endregion', pos)
        
        if region_start != -1 and region_start < region_end:
            depth += 1
            pos = region_start + 1
        elif region_end != -1:
            if depth == 0:
                end = region_end + len('\n        #endregion')
                # Also consume trailing newline
                if content[end:end+1] == '\n':
                    end += 1
                if content[end:end+1] == '\n':
                    end += 1
                content = content[:start] + content[end:]
                break
            else:
                depth -= 1
                pos = region_end + 1
        else:
            break

with open(r'E:\Users\Documents\GitHub\CryptoTool\CryptoTool.Win\EcdsaTabControl.cs', 'w', encoding='utf-8') as f:
    f.write(content)

# ===================== Process EcdsaTabControl.Ui.cs =====================
with open(r'E:\Users\Documents\GitHub\CryptoTool\CryptoTool.Win\EcdsaTabControl.Ui.cs', 'r', encoding='utf-8') as f:
    content = f.read()

# 1. Delete CreateLabelControlRow
old = '''        private static TableLayoutPanel CreateLabelControlRow(Label label, Control control)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            label.AutoSize = true;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Margin = new Padding(0, 0, 4, 0);

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0);

            panel.Controls.Add(label, 0, 0);
            panel.Controls.Add(control, 1, 0);
            return panel;
        }'''
content = content.replace(old, '')

# 2. Delete CreateFormatRow
old = '''        private static FlowLayoutPanel CreateFormatRow(Label inputLabel, ComboBox inputCombo, Label outputLabel, ComboBox outputCombo)
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                WrapContents = false
            };

            inputLabel.AutoSize = true;
            inputLabel.Margin = new Padding(0, 0, 2, 0);
            inputLabel.TextAlign = ContentAlignment.MiddleLeft;
            inputCombo.Margin = new Padding(0, 0, 16, 0);
            inputCombo.Width = 120;
            inputCombo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            outputLabel.AutoSize = true;
            outputLabel.Margin = new Padding(0, 0, 2, 0);
            outputLabel.TextAlign = ContentAlignment.MiddleLeft;
            outputCombo.Margin = new Padding(0);
            outputCombo.Width = 120;
            outputCombo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            panel.Controls.Add(inputLabel);
            panel.Controls.Add(inputCombo);
            panel.Controls.Add(outputLabel);
            panel.Controls.Add(outputCombo);
            return panel;
        }'''
content = content.replace(old, '')

# 3. Delete InitializeEncryptLayout (using brace counting)
start = content.find('        public void InitializeEncryptLayout()')
if start != -1:
    brace_start = content.find('{', start)
    if brace_start != -1:
        depth = 1
        pos = brace_start + 1
        while depth > 0 and pos < len(content):
            if content[pos] == '{':
                depth += 1
            elif content[pos] == '}':
                depth -= 1
            pos += 1
        if depth == 0:
            # pos is now after the closing }
            # consume trailing whitespace/newlines
            while pos < len(content) and content[pos] in ' \t\r\n':
                pos += 1
            content = content[:start] + content[pos:]

# 4. Delete InitializeEncryptCurveList
old = '''        public void InitializeEncryptCurveList()
        {
            if (_allCurveData.Count == 0)
                _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

            comboEncCurveCategory.DisplayMember = "Text";
            comboEncCurveCategory.ValueMember = "Value";
            foreach (var cat in _allCurveData)
            {
                comboEncCurveCategory.Items.Add(new
                {
                    Text = $"{cat.Value.Icon} {cat.Key}",
                    Value = cat.Key
                });
            }

            comboEncCurve.DisplayMember = "Value";
            comboEncCurve.ValueMember = "Key";

            comboEncCurveCategory.SelectedIndexChanged += ComboEncCurveCategory_SelectedIndexChanged;

            if (comboEncCurveCategory.Items.Count > 0)
                comboEncCurveCategory.SelectedIndex = 0;
        }'''
content = content.replace(old, '')

with open(r'E:\Users\Documents\GitHub\CryptoTool\CryptoTool.Win\EcdsaTabControl.Ui.cs', 'w', encoding='utf-8') as f:
    f.write(content)

print("Done")
