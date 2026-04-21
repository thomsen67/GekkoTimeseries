import pyarrow.parquet as pq
import pandas as pd
import matplotlib.pyplot as plt

parquet_file = pq.ParquetFile("c:\\Thomas\\Desktop\\gekko\\testing\\traces.parquet")
df = parquet_file.read_row_group(0).to_pandas()
print(len(df))

# Gcm med flest traces
print(df['commandFile'].value_counts().head(10))

# Databnak med flest traces
print(df['databankFile'].value_counts().head(10))

# Forekomst af 'obk' (lille bank med ganske få)
print(df['commandFile'].str.contains("uadam", case=False, na=False).sum())

# plot "sessions"
plt.scatter(df['stamp'], df['counter'], s=20, alpha=0.5, color='blue')
plt.show()

print("")