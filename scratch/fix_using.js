const fs = require('fs');
const path = require('path');

function walkDir(dir, callback) {
    fs.readdirSync(dir).forEach(f => {
        let dirPath = path.join(dir, f);
        let isDirectory = fs.statSync(dirPath).isDirectory();
        isDirectory ? walkDir(dirPath, callback) : callback(path.join(dir, f));
    });
}

walkDir('c:/Users/ADMIN/Desktop/DBMS/unit test', function(filePath) {
    if (filePath.endsWith('.cs')) {
        let content = fs.readFileSync(filePath, 'utf8');
        let newContent = content.replace(/using DBMS[^;]+;\r?\n/g, '');
        if (content !== newContent) {
            fs.writeFileSync(filePath, newContent, 'utf8');
        }
    }
});
