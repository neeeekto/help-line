const path = require("path");
const fs = require("fs");

const renameFiles = (dir, fileNameMaker = (currentName, pathToFolder) => null) => {
  let files = fs.readdirSync(dir);
  files.forEach((file) => {
    const pathToFolder = path.join(dir, file);
    const newName = fileNameMaker(file, pathToFolder);
    if (fs.statSync(pathToFolder).isDirectory()) {
      renameFiles(pathToFolder, fileNameMaker);
      if (newName) {
        fs.renameSync(pathToFolder, path.join(dir, newName));
      }
    } else {
      if (newName) {
        fs.renameSync(pathToFolder, path.join(dir, newName));
      }
    }
  });
};

const pattern = "HelpLine.Modules.Quality";
const newPattern = "HelpLine.Modules.System";
renameFiles(path.resolve('./src/Modules/System'), (name, pathToFolder) => {
    if(pathToFolder.includes('\\bin')) return;
    if(name.includes(pattern)) {
        return name.replace(pattern, newPattern);
    }
});
