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
parquet_file = "makrobk.parquet"
png_file1 = "fig1.png"
png_file2 = "fig2.png"

# ------- gdx via dreamtools -----------

s = dt.Gdx(f"{path}\{gdx_file}")
dt.time(t1, t2)
df1 = dt.DataFrame(
  [s.qBNP, s.vBNP],
  names=["BNP mængder", "BNP værdier"]
)
fig1 = df1.plot()
dt.write_image(fig1, f"{path}\{png_file1}", scale=1)

# ------- parquet -----------

import plotly.express as px

pg.run(f"read {path}\{gbk_file};")
pg.run(f"write <parquet> {path}\{parquet_file};")

if True:
    parquet_file = pq.ParquetFile(f"{path}\{parquet_file}") 
    df2a = parquet_file.read_row_group(0).to_pandas().drop(columns=['date', 'period', 'value'])
    df2b = parquet_file.read_row_group(1).to_pandas()[['id', 'date', 'period', 'value']]
    df2 = df2a.merge(df2b[['id']], on='id', how='right')
    df2[['date', 'period', 'value']] = df2b[['date', 'period', 'value']].values

vars = ["makrobk:qbnp!a", "makrobk:vbnp!a"]
fig2 = px.line(df2[df2["id"].isin(vars)], x="date", y="value", color="id")
fig2.update_layout(xaxis_title="År", yaxis_title="", legend_title="")
dt.write_image(fig2, f"{path}\{png_file2}", scale=1)




i = 100


