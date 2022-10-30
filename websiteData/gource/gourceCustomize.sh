# Remove actions after a certain date (currently December SGX)
cat gourceLog.txt | awk -F\| '$1<=1639785600' > gourceLog.temp
sed -i.bak '/Docs/d' ./gourceLog.temp
sed -i.bak '/Packages/d' ./gourceLog.temp
sed -i.bak '/Lux URP Essentials/d' ./gourceLog.temp
sed -i.bak '/Doxygen/d' ./gourceLog.temp
sed -i.bak '/InControl/d' ./gourceLog.temp
sed -i.bak '/SampleScenes/d' ./gourceLog.temp
sed -i.bak '/GrapplePRO/d' ./gourceLog.temp
sed -i.bak '/ProjectSettings/d' ./gourceLog.temp
sed -i.bak '/LeanTween/d' ./gourceLog.temp
sed -i.bak '/Plugins/d' ./gourceLog.temp
sed -i.bak '/Polybrush/d' ./gourceLog.temp
sed -i.bak '/ProBuilder/d' ./gourceLog.temp
sed -i.bak '/Standard Assets/d' ./gourceLog.temp
sed -i.bak '/Samples/d' ./gourceLog.temp
sed -i.bak '/TextMesh/d' ./gourceLog.temp
sed -i.bak '/\.meta/d' ./gourceLog.temp
sed -i.bak '/trunk\/obj/d' ./gourceLog.temp
mv gourceLog.temp gourceLog.txt
rm gourceLog.temp.bak

# Setup Project Name
projName="Re:Active - Unity 3d Project"

function fix {
  sed -i -- "s/$1/$2/g" gourceLog.txt
}

# Replace non human readable names with proper ones
fix "|berriers|" "|Prof. B|"
fix "|seaverj|" "|Prof. Seaver|"
fix "|crudelen4597|" "|Nicolas Crudele|"
fix "|reesej0892|" "|Jake Reese|"
fix "|butlerm2925|" "|Maggie Butler|"
fix "|samuelsonj3712|" "|John Samuelson|"
fix "|krohnc7348|" "|Christian Krohn|"
fix "|biggerstaffm2853|" "|Maxton Biggerstaff|"
fix "|odrichb1297|" "|Benjamin Odrich|"
fix "|jakopina9363|" "|Alec Jakopin|"
fix "|fortunh4866|" "|Henry Fortun|"
