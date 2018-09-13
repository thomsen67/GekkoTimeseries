set       i     /a,b,c/
parameter d2(i) /a 2, b 0 /;
parameter test;
test = d2('c');
execute_unload 'test', test;

