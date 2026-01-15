import pygekko as pg # pip install gekko
import pandas as pd # pip install pandas
import pyarrow.parquet as pq # pip install pyarrow

import gams # pip install gamsapi
import kaleido # pip install upgrade kaleido
import dreamtools as dt # pip install dream-tools

t1 = 1966
t2 = 2023
path = "c:\\Thomas\\Desktop\\gekko\\testing"
gdx_file = "makrobk.gdx"
gbk_file = "makrobk.gbk"
parquet_path = "makrobk.parquet"
png_file1 = "fig1.png"
png_file2 = "fig2.png"
png_file3 = "fig3.png"

# ------- gdx via dreamtools -----------

#s = dt.Gdx(f"{path}\{gdx_file}")
#dt.time(t1, t2)
#df1 = dt.DataFrame(
#  [s.qBNP, s.vBNP],
#  names=["BNP mængder", "BNP værdier"]
#)
#fig1 = df1.plot()
#dt.write_image(fig1, f"{path}\{png_file1}", scale=1)

# ------- parquet -----------

import plotly.express as px

# Danner df2 fra en .gdx-fil
#pg.run("option folder working = 'c:\\Thomas\\Desktop\\gekko\\testing';")
#pg.run(f"read <gdx> {path}\{gdx_file};")
#pg.run(f"write <parquet> {path}\{parquet_path};")
#parquet_file = pq.ParquetFile(f"{path}\{parquet_path}") 
#df2a = parquet_file.read_row_group(0).to_pandas().drop(columns=['date', 'period', 'value'])
#df2b = parquet_file.read_row_group(1).to_pandas()[['id', 'date', 'period', 'value']]
#df2 = df2a.merge(df2b[['id']], on='id', how='right')
#df2[['date', 'period', 'value']] = df2b[['date', 'period', 'value']].values

#pg.run(f"read <parquet> {path}\{parquet_path};")

#vars = ["makrobk:qbnp!a", "makrobk:vbnp!a"]
#print(df2)
#fig2 = px.line(df2[df2["id"].isin(vars)], x="date", y="value", color="id")
#fig2.update_layout(xaxis_title="År", yaxis_title="", legend_title="")
#dt.write_image(fig2, f"{path}\{png_file2}", scale=1)

op = ["p", "% p.a."]
pg.run(f"read <gdx> {path}\{gdx_file};")
pg.run(f"plot <2000 2020 {op[0]}> vBNP/qBNP, pC[ctot] file={path}\plot.parquet;")
parquet_file3 = pq.ParquetFile(f"{path}\plot.parquet") 
df3a = parquet_file3.read_row_group(0).to_pandas().drop(columns=['date', 'period', 'value'])
df3b = parquet_file3.read_row_group(1).to_pandas()[['id', 'date', 'period', 'value']]
df3 = df3a.merge(df3b[['id']], on='id', how='right')
df3[['date', 'period', 'value']] = df3b[['date', 'period', 'value']].values
#df3["date"] = pd.to_datetime(df3["date"], unit="ms")
#print(df3)
vars = ["x1!a", "x2!a"]
fig3 = px.line(df3[df3["id"].isin(vars)], x="date", y="value", color="label", hover_data=["id"])
fig3.update_layout(xaxis_title="", yaxis_title=op[1], legend_title="")
fig3.show()
dt.write_image(fig3, f"{path}\{png_file3}", scale=1)




i = 100


