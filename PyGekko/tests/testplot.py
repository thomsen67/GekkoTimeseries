#import gams # pip install gamsapi
#import kaleido # pip install upgrade kaleido
import dreamtools as dt # pip install dream-tools
import pandas as pd # pip install pandas
import pygekko as pg # pip install gekko
import plotly.express as px
from pathlib import Path

t1 = 1966; t2 = 2023
op = "p"
vars = "vBNP/qBNP, pC[ctot]"
path = "c:\\Thomas\\Desktop\\gekko\\testing"
data_file = "makrobk.gdx"
data_type = Path(data_file).suffix[1:]
png_name = "plot.png"
parquet_file = "plot.parquet"

pg.run(f"read <{data_type}> {path}\{data_file};")
pg.run(f"plot <{t1} {t2} {op[0]}> {vars} file={path}\{parquet_file};")
df = pg.read_parquet(f"{path}\{parquet_file}")
vars = df["id"].unique() # all plot variables, else pick them like ["x2!a", "x4!a", ...]
fig = px.line(df[df["id"].isin(vars)], x="date", y="value", color="label", hover_data=["id"])
fig.update_layout(xaxis_title="", yaxis_title="<"+op+">", legend_title="")
#fig.show()
dt.write_image(fig, f"{path}\{png_name}", scale=1)

i = 100


