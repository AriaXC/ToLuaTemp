-- Sting 工具扩展类
string = string or {}

---替换字符
---args里的key全部换成value
function string.replace( args,key,value )
	local  index = string.find(args,key)
	if index ~=nil then
		local  str = string.replace(string.sub(args,index+1,string.len(args)),key,value)
		args = string.sub(args,1,index-1)..value..str
	end
	return args
end