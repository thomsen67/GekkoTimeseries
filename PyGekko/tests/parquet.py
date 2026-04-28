import pyarrow.parquet as pq
import pandas as pd
import matplotlib.pyplot as plt

parquet_file = pq.ParquetFile("c:\\Thomas\\Desktop\\gekko\\testing\\sletmig.parquet")
df0 = parquet_file.read_row_group(0).to_pandas().drop(columns=['date', 'period', 'value'])
df1 = parquet_file.read_row_group(1).to_pandas()[['id', 'date', 'period', 'value']]
df = df0.merge(df1[['id']], on='id', how='right')
df[['date', 'period', 'value']] = df1[['date', 'period', 'value']].values
print(df)

print()
print("Gekko parquet version:  " + parquet_file.metadata.metadata.get(b"parquet.design.version").decode("utf-8"))
print("Gekko export timestamp: " + parquet_file.metadata.metadata.get(b"export.timestamp").decode("utf-8"))

# Plot
for name, group in df.groupby("id"):
    plt.plot(group["date"], group["value"], label=name, marker = 'o', markersize=4)
plt.xlabel("Date")
plt.ylabel("Value")
plt.title("Plot")
plt.legend()
plt.xticks(rotation=45)
plt.show()
x = 1