import subprocess
from pathlib import Path
import os
import sys

os.chdir(sys.argv[1])

PIPE = subprocess.PIPE

process = subprocess.Popen(['gitversion', '/showvariable', 'AssemblySemVer'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

version = stdoutput.decode("utf-8")
version = version.replace("\n", "")
version = version.replace("\r", "")

process = subprocess.Popen(['gitversion', '/showvariable', 'FullSemVer'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

branch = stdoutput.decode("utf-8")
branch = branch.replace("\n", "")
branch = branch.replace("\r", "")

process = subprocess.Popen(['gitversion', '/showvariable', 'ShortSha'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

branch2 = stdoutput.decode("utf-8")
branch2 = branch2.replace("\n", "")
branch2 = branch2.replace("\r", "")

process = subprocess.Popen(['gitversion', '/showvariable', 'AssemblySemFileVer'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

fileversion = stdoutput.decode("utf-8")
fileversion = version.replace("\n", "")
fileversion = version.replace("\r", "")

process = subprocess.Popen(['gitversion', '/showvariable', 'Major'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

major = stdoutput.decode("utf-8")
major = major.replace("\n", "")
major = major.replace("\r", "")

print("Major:" + major)

process = subprocess.Popen(['gitversion', '/showvariable', 'Minor'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

minor = stdoutput.decode("utf-8")
minor = minor.replace("\n", "")
minor = minor.replace("\r", "")

print("Minor:" + minor)

process = subprocess.Popen(['gitversion', '/showvariable', 'Patch'], stdout=PIPE, stderr=PIPE)
stdoutput, stderroutput = process.communicate()

patch = stdoutput.decode("utf-8")
patch = patch.replace("\n", "")
patch = patch.replace("\r", "")

print("Patch:" + patch)

productversion = major + "." + minor + "." + patch
branch = branch + "." + branch2

print("Version:" + version)
print("FileVersion:" + fileversion)
print("Branch:" + branch)
print("ProductVersion:" + productversion)

allVersion = branch

print("AllVersion:" + allVersion)

for file_path in Path('.').glob('**/GlobalAssemblyInfo.cs.template'):
    allPath = str(file_path)
    print("AllPath:" + allPath) 
    with open(allPath) as f:
        with open(allPath.replace(".template", ""), "w+") as out:
            for line in f:
                line = line.replace("@@AssemblyInformationalVersion@@", allVersion)
                line = line.replace("@@AssemblyVersion@@", version)
                line = line.replace("@@AssemblyFileVersion@@", fileversion)
                out.write(line)


# for file_path in Path('.').glob('**/package.json'):
#     allPath = str(file_path)
#     if "node_modules" in allPath:
#         continue
#     if "bin" in allPath:
#         continue
#     if "obj" in allPath:
#         continue
#     print("AllPath:" + allPath) 
#     changeIt = True
#     with open(allPath) as f:
#         with open(allPath.replace(".json", ".tmp"), "w+") as out:
#             for line in f:
#                 if line.startswith("  \"version\":") and changeIt:
#                     if productversion in line:
#                         changeIt = False
#                         continue
#                     line = "  \"version\": \"" + productversion + "\",\n"
#                 out.write(line)
#     if changeIt:
#         os.remove(allPath)
#         os.rename(allPath.replace(".json", ".tmp"), allPath)