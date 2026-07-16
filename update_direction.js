const fs = require('fs');

const readmePath = 'README.md';
let content = fs.readFileSync(readmePath, 'utf8');

content = content.replace(/direction LR/g, 'direction TB');

fs.writeFileSync(readmePath, content, 'utf8');
console.log('Successfully updated README.md to use vertical layout.');
