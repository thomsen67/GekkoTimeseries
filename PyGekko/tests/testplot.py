#import gams # pip install gamsapi
#import kaleido # pip install upgrade kaleido
import dreamtools as dt # pip install dream-tools
import pandas as pd # pip install pandas
import pygekko as pg # pip install pygekko
# pip install pyarrow

import plotly.express as px
from pathlib import Path

# Setup params
folder = "c:\\Thomas\\Desktop\\gekko\\testing"
t1 = 1966; t2 = 2023
op = "p"
vars = ["vBNP/qBNP", "pC[cTot]"]
png_name = "plot.png"
data_file = "makrobk.gdx"
data_type = Path(data_file).suffix[1:]

# Create a plot as a dataframe
pg.run(f"option folder working = '{folder}';")
pg.run(f"read <{data_type}> {data_file};")
df = pg.df_plot(f"<{t1} {t2} {op}> {", ".join(vars)}")

# Plot the dataframe via Plotly
ids = df["id"].unique() # all plot variables, else pick them like ["x2!a", "x4!a", ...]
fig = px.line(df[df["id"].isin(ids)], x="date", y="value", color="label", hover_data=["id"])
fig.update_layout(xaxis_title="", yaxis_title="<"+op+">", legend_title="")
#fig.show()
dt.write_image(fig, f"{folder}\{png_name}", scale=1)
print(f"Created plot {folder}\{png_name}")

