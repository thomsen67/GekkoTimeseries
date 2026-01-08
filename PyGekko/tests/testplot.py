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
png_file1 = "fig1.png"
png_file2 = "fig2.png"
png_file3 = "fig3.png"

#dt.REFERENCE_DATABASE = dt.Gdx(f"{path}\{gdx_file}")
s = dt.Gdx(f"{path}\{gdx_file}")

dt.time(t1, t2)
df1 = dt.DataFrame(
  [s.qBNP, s.vBNP],
  names=["BNP mængder", "BNP værdier"]
)
fig1 = df1.plot()
dt.write_image(fig1, f"{path}\{png_file1}", scale=1)



parquet_file = pq.ParquetFile("c:\\Thomas\\Desktop\\gekko\\testing\\makrobk.parquet") 
gdf0 = parquet_file.read_row_group(0).to_pandas().drop(columns=['date', 'period', 'value'])
gdf1 = parquet_file.read_row_group(1).to_pandas()[['id', 'date', 'period', 'value']]
gdf = gdf0.merge(gdf1[['id']], on='id', how='right')
gdf[['date', 'period', 'value']] = gdf1[['date', 'period', 'value']].values
print(gdf) 


import plotly.express as px
#df3 = pd.DataFrame({
#    "t": [2001, 2002, 2003],
#    "x": [100, 110, 90]
#})
#fig3 = px.line(df3, x="t", y="x", title="Time Series of x over t")
fig3 = px.line(gdf[gdf["id"] == "makrobk:qbnp!a"], x="date", y="value", title="Time Series of x over t")
dt.write_image(fig3, f"{path}\{png_file3}", scale=1)

i = 100


