import sys

def sync_comments(reference_file, target_file, output_file):
    with open(reference_file, 'r', encoding='utf-8') as ref:
        ref_lines = ref.readlines()
    
    with open(target_file, 'r', encoding='utf-8') as tgt:
        tgt_lines = tgt.readlines()
    
    max_lines = max(len(ref_lines), len(tgt_lines))
    result_lines = []
    
    for i in range(max_lines):
        ref_line = ref_lines[i] if i < len(ref_lines) else ''
        tgt_line = tgt_lines[i] if i < len(tgt_lines) else ''
        
        ref_stripped = ref_line.strip()
        tgt_stripped = tgt_line.strip()
        
        if ref_stripped.startswith('//') or ref_stripped.startswith('///'):
            result_lines.append(ref_line)
        else:
            result_lines.append(tgt_line)
    
    with open(output_file, 'w', encoding='utf-8') as out:
        out.writelines(result_lines)

if __name__ == '__main__':
    ref_path = r'CryptoTool.Win\.案例代码【腾讯元宝整理】\整理2\EcdsaTabControl.Designer.cs'
    tgt_path = r'CryptoTool.Win\EcdsaTabControl.Designer.cs'
    sync_comments(ref_path, tgt_path, tgt_path)
    print('注释同步完成！')