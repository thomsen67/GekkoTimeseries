import pygekko as pg

# If windows are laggy, consider setting this: pg.threads(True)

t1 = 2028
t2 = 2035

op = "d"

pg.run("tell gekkoinfo('short4');")
pg.run("help decomp;")
pg.run("option folder working = 'c:\\Thomas\\Desktop\\gekko\\testing';")
pg.run("%x = 2;")
pg.run("prt %x;")
pg.run(f"reset; time {t1} {t2};")

pg.run("x = 12345;")
pg.run("write sletmig;")
pg.run("read sletmig;")
pg.run("prt x;")

pg.run("prt x;")
pg.run("write <parquet> sletmig;")
pg.run("read <parquet> sletmig;")
pg.run("read makro.gbk;")

pg.run("prt <p> qBNP, vBNP/qBNP;")
pg.run(f"plot <{t1} {t2} {op}> qBNP, vBNP/qBNP;")
pg.plot(["qBNP", "vBNP/qBNP"], t=[t1, t2], op=op)
pg.run("model <gms> makro.zip;")
pg.run("prt qBNP/2;")
pg.run(f"decomp <{t1} {t2} {op}> qBNP from E_qBNP;")
pg.decomp("qBNP", t=[t1, t2], op=op, from_="E_qBNP")
pg.run("prt qBNP/3;")
pg.wait()
