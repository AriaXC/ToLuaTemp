-- Sting 工具扩展类
string = string or {}

---替换字符
---args里的key全部换成value
---keyLength 有转义字符的时候 用到
function string.replace( args,key,value,keyLength)
	local  index = string.find(args,key)
	local  len = #key
	if keyLength then
		 len = keyLength
	end

	if index ~=nil then
		local  str = string.replace(string.sub(args,index+len,#args),key,value,keyLength)
		args = string.sub(args,1,index-1)..value..str
	end
	return args
end

