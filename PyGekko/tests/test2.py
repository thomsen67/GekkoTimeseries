import pygekko as pg
import pandas as pd
t1 = 2021
t2 = 2023
pg.run("reset;")
pg.run(f"time {t1} {t2};")
pg.run("x1 = 2, 3, 4;")
pg.run("x2 = 7, 5, 6;")
pg.run("y = x1 + x2;")
pg.run("prt x1, x2, x1/y;")

import pyarrow.parquet as pq
 
parquet_file = pq.ParquetFile("c:\\Thomas\\Desktop\\gekko\\testing\\makrobk.parquet")
 
df0 = parquet_file.read_row_group(0).to_pandas().drop(columns=['date', 'period', 'value'])
df1 = parquet_file.read_row_group(1).to_pandas()[['id', 'date', 'period', 'value']]
df = df0.merge(df1[['id']], on='id', how='right')
df[['date', 'period', 'value']] = df1[['date', 'period', 'value']].values
print(df)

 
print()
print("Gekko parquet version:  " + parquet_file.metadata.metadata.get(b"parquet.design.version").decode("utf-8"))
print("Gekko export timestamp: " + parquet_file.metadata.metadata.get(b"export.timestamp").decode("utf-8"))

