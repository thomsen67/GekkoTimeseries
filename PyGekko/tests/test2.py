import pygekko as pg
t1 = 2021
t2 = 2023
pg.run("reset;")
pg.run(f"time {t1} {t2};")
pg.run("x1 = 2, 3, 4;")
pg.run("x2 = 7, 5, 6;")
pg.run("y = x1 + x2;")
pg.run("prt x1, x2, x1/y;")
