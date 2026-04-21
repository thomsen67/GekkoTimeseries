using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gekko
{
    class Visualizer
    {
        public static void Run()
        {
            string inputPath = @"c:\Thomas\Desktop\gekko\testing\Fremdrift\DatOpSekvens2.txt"; // Din kildefil
            string outputPath = @"c:\Thomas\Desktop\gekko\testing\Fremdrift\DatOpSekvens2.html";
            string outputPathCsv = @"c:\Thomas\Desktop\gekko\testing\Fremdrift\DatOpSekvens2.csv";
            if (!File.Exists(inputPath)) new Error("Hov");

            var lines = File.ReadAllLines(inputPath);
            var blocks = ParseData(lines);

            if (true)
            {
                StringBuilder csv = new StringBuilder();
                foreach (var block in blocks)
                {
                    csv.Append(block.Id + " " + block.Header + "; ");
                    csv.Append(block.Kb + "; ");
                    csv.Append(block.Procent + "; ");
                    csv.Append(block.Kompleksitet + "; ");
                    TwoStrings two = ADAMVars(block);
                    csv.Append(two.s1.ToString() + "; ");
                    csv.Append(two.s2.ToString() + "; ");
                    int i = -1;
                    foreach (string s5 in block.Paths)
                    {
                        string comma = null;
                        i++;
                        if (i < block.Paths.Count - 1) comma = ", ";
                        string s = s5;
                        if (s5.StartsWith("* ")) s = s5.Substring(2);
                        if (s.Contains("  "))
                        {
                            var match = Regex.Match(s, @"^(.*?)\s+(\d{2}/\d{2}\s+\d{4})");
                            if (match.Success)
                            {
                                string path = match.Groups[1].Value;
                                string date = match.Groups[2].Value;
                                string result = $"{path} ({date})";
                                csv.Append(result + comma);
                            }
                            else
                            {
                                csv.Append("<error>" + comma);
                            }
                        }
                        else
                        {
                            csv.Append(s + comma);
                        }
                    }
                    //csv.Append(Stringlist.GetListWithCommas(block.Paths) + "; ");
                    csv.AppendLine("");
                }
                File.WriteAllText(outputPathCsv, csv.ToString(), new UTF8Encoding(true));
            }

            string html = GenerateHtml(blocks);

            File.WriteAllText(outputPath, html);
            new Writeln("HTML er genereret i: " + outputPath);
        }

        static string GenerateHtml(List<DataBlock> blocks)
        {
            var sb = new StringBuilder();

            // --- BEREGNINGER ---
            double totalKb = blocks.Sum(b => Kb(b));
            double totalWeightedProgress = blocks.Sum(b => (double)b.Procent * Kb(b));
            double aggregatePercent = totalKb > 0 ? totalWeightedProgress / totalKb : 0;

            Func<int, int> colorPriority = (p) => {
                if (p < 25) return 0;
                if (p < 50) return 1;
                if (p < 75) return 2;
                return 3;
            };

            var sortedForMaster = blocks
                .OrderBy(b => colorPriority(b.Procent))
                .ThenByDescending(b => Kb(b))
                .ToList();

            sb.Append("<!DOCTYPE html><html><head><meta charset='UTF-8'>");
            sb.Append("<style>");
            sb.Append("body { font-family: verdana; background: #f4f4f4; padding: 0px 20px 20px 20px; scroll-behavior: smooth; }");
            sb.Append(".master-container { background: #333; color: white; border-radius: 8px; padding: 20px; margin-bottom: 20px; position: sticky; top: 10px; z-index: 1000; box-shadow: 0 4px 10px rgba(0,0,0,0.3); }");
            sb.Append(".master-bar { background: #555; height: 35px; width: 100%; display: flex; margin-top: 10px; border: 1px solid #000; overflow: hidden; }");
            sb.Append(".master-segment { height: 100%; border-right: 0.5px solid #333; box-sizing: border-box; cursor: pointer; }");
            sb.Append(".master-segment:hover { opacity: 0.8; border: 1px solid white; }");

            sb.Append(".info-box { background: #ddd; margin-bottom: 20px; box-shadow: 2px 2px 5px rgba(0,0,0,0.05); overflow: hidden; }");
            sb.Append(".info-header-wrapper { display: flex; justify-content: space-between; align-items: center; padding: 5px 15px; }");
            sb.Append("summary { cursor: pointer; font-weight: bold; color: #0056b3; outline: none; font-size: 0.8em; flex-grow: 1; }");
            sb.Append(".info-content { padding: 15px; font-size: 0.8em; line-height: 1.4; color: #333; border-top: 1px solid #ccc; background: #ddd; }");

            sb.Append(".inline-controls { font-size: 0.8em; display: flex; align-items: center; gap: 10px; }");
            sb.Append(".block { scroll-margin-top: 180px; background: white; border: 1px solid #ccc; margin-bottom: 20px; padding: 15px; border-radius: 8px; box-shadow: 2px 2px 5px rgba(0,0,0,0.1); }");
            sb.Append(".header { font-size: 1.2em; margin-bottom: 10px; display: block; }");
            sb.Append(".path-container { background: #FFF9F5; padding: 5px; max-height: 4.5em; overflow-y: auto; border: 1px solid #eee; margin: 10px 0; font-family: monospace; font-size: 0.9em; white-space: pre-wrap; }");
            sb.Append(".vars-container { background: #FFF9F5; padding: 8px; overflow-x: auto; white-space: nowrap; border: 1px solid #eee; margin-bottom: 10px; font-family: monospace; font-size: 0.9em;}");
            sb.Append(".progress-bg { background: #ddd; height: 20px; margin: 10px 0; overflow: hidden; display: flex; }");
            sb.Append(".progress-fill { height: 100%; }");
            sb.Append("</style>");

            sb.Append("<script>");
            sb.Append("function switchOrder(type) {");
            sb.Append("  document.getElementById('list-chrono').style.display = (type === 'chrono' ? 'block' : 'none');");
            sb.Append("  document.getElementById('list-sorted').style.display = (type === 'sorted' ? 'block' : 'none');");
            sb.Append("}");
            sb.Append("function jumpToBlock(id) {");
            sb.Append("  var isSorted = document.getElementById('list-sorted').style.display === 'block';");
            sb.Append("  var targetId = (isSorted ? 'list-sorted-' : 'list-chrono-') + id;");
            sb.Append("  var el = document.getElementById(targetId);");
            sb.Append("  if(el) el.scrollIntoView({behavior:'smooth'});");
            sb.Append("}");
            sb.Append("</script></head><body>");

            // Master Bar
            sb.Append("<div class='master-container'>");
            sb.Append("<span style='font-size: 1.5em; font-weight: bold;'>Kildeprojekt: status</span>");
            sb.Append($"<div style='margin-top: 5px;'>Vægtet færdiggørelse: <b>{aggregatePercent:F1}%</b></div>");
            sb.Append("<div class='master-bar'>");

            double visibleKb = sortedForMaster.Where(b => b.Kb > 1).Sum(b => Kb(b));

            foreach (var b in sortedForMaster)
            {
                if (b.Kb <= 1) continue;
                double widthPct = Kb(b) / visibleKb * 100;
                string color = GetColor(b.Procent);
                sb.Append($"<div class='master-segment' style='width: {widthPct:F4}%; background-color: {color};' " +
                          $"title='{b.Id}: {b.Header}\n[kb]: {b.Kb}\n[procent]: {b.Procent}%\n[kompleksitet]: {b.Kompleksitet}\nKlik for at gå til submodul' " +
                          $"onclick=\"jumpToBlock('{b.Id}')\"></div>");
            }
            sb.Append("</div></div>");

            // --- INFO OG CONTROLS (Samlet i én bar) ---
            sb.Append("<div class='info-box'>");
            sb.Append("<details>");
            sb.Append("<summary>");
            sb.Append("<div class='info-header-wrapper' style='display:inline-flex; width:95%;'>");
            sb.Append("<span>Mere information</span>");
            sb.Append("<div class='inline-controls' onclick='event.stopPropagation();'>");
            sb.Append("<b style='color:black'>Sortering: </b>");
            sb.Append("<input type='radio' name='sort' id='r1' checked onclick=\"switchOrder('chrono')\"> <label for='r1'>Kronologisk</label>");
            sb.Append("<input type='radio' name='sort' id='r2' onclick=\"switchOrder('sorted')\"> <label for='r2'>Efter status</label>");
            sb.Append("</div></div></summary>");
            sb.Append("<div class='info-content'>");
            sb.Append("<p>Denne html-side viser fremdrift i kildeprojektet, herunder submodulers status.");
            sb.Append("<ul>");
            sb.Append("<li>Klik på et farvet segment i statuslinjen for at navigere til modulet. Hover for at se info.</li>");
            sb.Append("<li>Statuslinjen afspejler KB-størrelse (gcm-filer) samt færdiggørelsesprocent (rød/orange/gul/grøn).</li>");
            sb.Append("<li>Samlet færdiggørelsesprocent snyder nok lidt, afhængigt af hvor meget der skal tages fra bl.a. ESTBK/UADAM-dele.</li>");
            sb.Append("<li>Oversigten er lavet ud fra data-traces til en forårs-databank. Der kan være unøagtigheder mht. denne metode.</li>");
            sb.Append("<li>Løbenumrene på de enkelte komponenter har ingen betydning i sig selv.</li>");
            sb.Append("<li>For hvert modul er der givet en liste over de ADAM-variabler, som dannes i modulet. Her er der kun medtaget de ca. 2000 ADAM-variabler som er relevante for MAKROBK. Vær opmærksom på, at et modul godt kan producere hjælpevariabler til andre moduler. ADAM-variabler kan have forskellig håndtering for forskellige tidsperioder.</li>");
            sb.Append("<li>De følgende moduler er ikke på listen, men er nævnt i MOLs oversigtsregneark: \\pension, \\divbanker\\DNFP, \\divbanker\\DNSOSB_og_T, \\05banker\\saerlige, \\basis (prebasis og postbasis), \\kontrol.</li>");
            sb.Append("</ul>");
            sb.Append("</div></details></div>");

            // Generer listerne med unikke præfiks-ID'er
            GenerateList(sb, blocks, "list-chrono", true);
            GenerateList(sb, sortedForMaster, "list-sorted", false);

            sb.Append("</body></html>");
            return sb.ToString();
        }

        private static void GenerateList(StringBuilder sb, List<DataBlock> list, string divId, bool visible)
        {
            string display = visible ? "block" : "none";
            sb.Append($"<div id='{divId}' style='display: {display};'>");
            foreach (var b in list)
            {
                // Unikt ID for at undgå hop-konflikt mellem lister
                sb.Append($"<div class='block' id='{divId}-{b.Id}'>");
                sb.Append($"<span class='header'><b>{b.Id}: {b.Header}</b></span>");

                sb.Append("<div class='path-container'>");
                foreach (var path in b.Paths) sb.Append($"{path}\n"); // white-space: pre-wrap bevarer blanks
                sb.Append("</div>");
                TwoStrings two = ADAMVars(b);
                sb.Append($"<div class='vars-container'>{two.s1 + ":" + two.s2}</div>");

                string color = GetColor(b.Procent);
                sb.Append($"<div style='font-size: 0.7em'>[kb]: {b.Kb}</div>");
                sb.Append($"<div style='font-size: 0.7em'>[procent]: {b.Procent}%</div>");
                sb.Append($"<div style='font-size: 0.7em'>[kompleksitet]: {b.Kompleksitet}</div>");
                sb.Append($"<div class='progress-bg' style='width:{3 * Kb(b)}px;'>");
                sb.Append($"<div class='progress-fill' style='width:100%; background-color:{color};'></div>");
                sb.Append("</div></div>");
            }
            sb.Append("</div>");
        }

        private static TwoStrings ADAMVars(DataBlock b)
        {
            var varList = b.Vars?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
            int count = (b.Vars != null && b.Vars.Contains("<ingen>")) ? 0 : varList.Length;
            TwoStrings two = new TwoStrings(count.ToString(), b.Vars);
            return two;
        }

        static List<DataBlock> ParseData(string[] lines)
        {            
            var blocks = new List<DataBlock>();
            DataBlock currentBlock = null;
            string previousLine = "";

            foreach (var line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;
                var match = Regex.Match(trimmed, @"^-+\s*(\d+[a-zA-Z]?)\s*-+$");
                if (match.Success)
                {
                    currentBlock = new DataBlock { Id = match.Groups[1].Value, Header = previousLine };
                    blocks.Add(currentBlock);
                    continue;
                }
                if (currentBlock != null)
                {
                    if (trimmed.StartsWith("[vars]:")) currentBlock.Vars = trimmed.Replace("[vars]:", "").Trim();
                    else if (trimmed.StartsWith("[kb]:")) currentBlock.Kb = int.Parse(trimmed.Replace("[kb]:", ""));
                    else if (trimmed.StartsWith("[procent]:")) currentBlock.Procent = int.Parse(trimmed.Replace("[procent]:", ""));
                    else if (trimmed.StartsWith("[kompleksitet]:")) currentBlock.Kompleksitet = trimmed.Replace("[kompleksitet]:", "");
                    else if (trimmed.Contains(@":\") || trimmed.Contains(@":/")) currentBlock.Paths.Add(trimmed);
                }
                previousLine = trimmed;
            }
            return blocks;
        }

        private static double Kb(DataBlock b)
        {
            double k = 1d;
            if (b.Kompleksitet == "1") k = 0.67;
            else if (b.Kompleksitet == "3") k = 1.5;
            return k * (double)b.Kb;
        }

        static string GetColor(int pct)
        {
            if (pct < 25) return "red";
            if (pct < 50) return "orange";
            if (pct < 75) return "yellow";
            return "green";
        }
    }

    class DataBlock
    {
        public string Id { get; set; }
        public string Header { get; set; }
        public List<string> Paths { get; set; } = new List<string>();
        public string Vars { get; set; }
        public int Kb { get; set; }
        public int Procent { get; set; }
        public string Kompleksitet { get; set; }
    }
}