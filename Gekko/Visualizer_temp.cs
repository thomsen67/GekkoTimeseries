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
            if (!File.Exists(inputPath)) new Error("Hov");            

            var lines = File.ReadAllLines(inputPath);
            var blocks = ParseData(lines);
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

            // INVERSE prioritet (Rød er vigtigst nu)
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
            sb.Append("body { font-family: verdana; background: #f4f4f4; padding: 20px; scroll-behavior: smooth; }");

            // Sticky bar - vi sikrer den har en fast højde så scroll-margin passer
            sb.Append(".master-container { background: #333; color: white; border-radius: 8px; padding: 20px; margin-bottom: 20px; position: sticky; top: 10px; z-index: 1000; box-shadow: 0 4px 10px rgba(0,0,0,0.3); height: 100px; }");
            sb.Append(".master-bar { background: #555; height: 35px; width: 100%; display: flex; margin-top: 10px; border: 1px solid #000; overflow: hidden; }");
            sb.Append(".master-segment { height: 100%; border-right: 0.5px solid #333; box-sizing: border-box; cursor: pointer; }");
            sb.Append(".master-segment:hover { opacity: 0.8; border: 1px solid white; }");

            sb.Append(".info-box { background: #ddd; border-radius: 8px; padding: 10px; margin-bottom: 20px; box-shadow: 2px 2px 5px rgba(0,0,0,0.05); }");            
            sb.Append(".info-box summary { cursor: pointer; font-weight: bold; color: #0056b3; outline: none; padding: 5px; font-size: 0.8em; }");
            sb.Append(".info-content { padding: 10px; font-size: 0.8em; line-height: 1.4; color: #333; border-top: 1px solid; margin-top: 5px; }");

            sb.Append(".controls { background: #ddd; padding: 10px; border-radius: 8px; margin-bottom: 20px; display: inline-block; }");

            // VIGTIGT: scroll-margin-top sørger for at blokken lander under den sticky bar
            sb.Append(".block { scroll-margin-top: 180px; background: white; border: 1px solid #ccc; margin-bottom: 20px; padding: 15px; border-radius: 8px; box-shadow: 2px 2px 5px rgba(0,0,0,0.1); }");
            sb.Append(".header { font-size: 1.2em; margin-bottom: 10px; display: block; }");
            sb.Append(".path-container { background: #FFF9F5; padding: 5px; max-height: 4.5em; overflow-y: auto; border: 1px; margin: 10px 0; font-family: monospace; font-size: 0.9em; white-space: pre-wrap; }");
            sb.Append(".vars-container { background: #FFF9F5; padding: 8px; overflow-x: auto; white-space: nowrap; border: 1px; margin-bottom: 10px; font-family: monospace;  font-size: 0.9em;}");
            sb.Append(".progress-bg { background: #ddd; height: 20px; margin: 10px 0; overflow: hidden; display: flex; }");
            sb.Append(".progress-fill { height: 100%; }");
            sb.Append("</style>");

            sb.Append("<script>");
            sb.Append("function switchOrder(type) {");
            sb.Append("  document.getElementById('list-chrono').style.display = (type === 'chrono' ? 'block' : 'none');");
            sb.Append("  document.getElementById('list-sorted').style.display = (type === 'sorted' ? 'block' : 'none');");
            sb.Append("}");
            sb.Append("</script></head><body>");

            // Master Bar
            sb.Append("<div class='master-container'>");
            sb.Append("<span style='font-size: 1.5em; font-weight: bold;'>Kildeprojekt: status</span>");
            sb.Append($"<div style='margin-top: 5px;'>Vægtet færdiggørelse: <b>{aggregatePercent:F1}%</b></div>");
            sb.Append("<div class='master-bar'>");

            double visibleKb = sortedForMaster.Where(b => b.Procent > 1).Sum(b => Kb(b));            

            foreach (var b in sortedForMaster)
            {                
                //if (b.Procent <= 1) continue;
                //double widthPct = (Kb(b) / totalKb) * 100;
                double widthPct = Kb(b) / visibleKb * 100;
                string color = GetColor(b.Procent);
                sb.Append($"<div class='master-segment' style='width: {widthPct:F4}%; background-color: {color};' " +
                          $"title='{b.Id}: {b.Header}\n[kb]: {b.Kb}\n[procent]: {b.Procent}\n[kompleksitet]: {b.Kompleksitet}\nKlik for visning'" +
                          $"onclick=\"location.href='#block-{b.Id}'\"></div>");
            }
            sb.Append("</div></div>");

            // --- INFO TOGGLE ---
            sb.Append("<div class='info-box'>");
            sb.Append("<details>");
            sb.Append("<summary>Mere information</summary>");
            sb.Append("<div class='info-content'>");
            sb.Append("<strong>Vejledning:</strong><br>");
            sb.Append("Dette dashboard viser fremdriften af datakonverteringen. ");
            sb.Append("Den øverste bjælke viser det vægtede gennemsnit baseret på KB-størrelse.<br><br>");
            sb.Append("<ul>");
            sb.Append("<li><b>Klik på baren:</b> Spring direkte til en specifik blok.</li>");
            sb.Append("<li><b>Sortering:</b> Skift mellem kronologisk rækkefølge eller prioriteret visning (rød først).</li>");
            sb.Append("<li><b>Filtrering:</b> Blokke med 1% eller mindre fremdrift er skjult i oversigtsbaren for at give et bedre overblik.</li>");
            sb.Append("</ul>");
            sb.Append("</div>");
            sb.Append("</details>");
            sb.Append("</div>");

            // Controls
            sb.Append("<div class='controls'>");
            sb.Append("<b style='font-size: 0.8em'>Sortering af submoduler: </b>");
            sb.Append("<input type='radio' name='sort' id='r1' checked onclick=\"switchOrder('chrono')\"> <label for='r1' style='font-size: 0.8em'>I opdateringsrækkefølge</label> ");
            sb.Append("<input type='radio' name='sort' id='r2' onclick=\"switchOrder('sorted')\"> <label for='r2' style='font-size: 0.8em'>Som statuslinjen</label>");
            sb.Append("</div>");

            // Listerne (Nu sorteret med Rød først i den ene)
            GenerateList(sb, blocks, "list-chrono", true);
            GenerateList(sb, sortedForMaster, "list-sorted", false);

            sb.Append("</body></html>");
            return sb.ToString();
        }

        // Hjælpefunktion til at bygge selve blok-listen
        private static void GenerateList(StringBuilder sb, List<DataBlock> list, string divId, bool visible)
        {
            string display = visible ? "block" : "none";
            sb.Append($"<div id='{divId}' style='display: {display};'>");
            foreach (var b in list)
            {
                // ID sat her til navigation
                sb.Append($"<div class='block' id='block-{b.Id}'>");
                sb.Append($"<span class='header'><b>{b.Id}: {b.Header}</b></span>");

                sb.Append("<div class='path-container'>");
                foreach (var path in b.Paths)
                {                    
                    sb.Append($"{path}<br>");
                }
                sb.Append("</div>");

                var varList = b.Vars?.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
                int count = (b.Vars != null && b.Vars.Contains("<ingen>")) ? 0 : varList.Length;
                string arrow = count > 0 ? ": " : null;
                sb.Append($"<div class='vars-container'>{count}{" ADAM-vars"}{arrow}{b.Vars}</div>");

                string color = GetColor(b.Procent);
                sb.Append($"<div style='font-size: 0.7em'>[kb]: {b.Kb}</div>");
                sb.Append($"<div style='font-size: 0.7em'>[procent]: {b.Procent}%</div>");
                sb.Append($"<div style='font-size: 0.7em'>[kompleksitet]: {b.Kompleksitet}</div>");

                sb.Append($"<div class='progress-bg' style='width:{3 * Kb(b)}px;'>");
                sb.Append($"<div class='progress-fill' style='width:100%; background-color:{color};'></div>");
                sb.Append("</div>");
                sb.Append("</div>");
            }
            sb.Append("</div>");
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

                // Tjek for separator: ------------- 1 -----------------                
                var match = Regex.Match(trimmed, @"^-+\s*(\d+[a-zA-Z]?)\s*-+$");
                if (match.Success)
                {
                    currentBlock = new DataBlock
                    {
                        Id = match.Groups[1].Value,
                        Header = previousLine // Linjen før separatoren
                    };
                    blocks.Add(currentBlock);
                    continue;
                }

                if (currentBlock != null)
                {
                    if (trimmed.StartsWith("[vars]:"))
                        currentBlock.Vars = trimmed.Replace("[vars]:", "").Trim();
                    else if (trimmed.StartsWith("[kb]:"))
                        currentBlock.Kb = int.Parse(trimmed.Replace("[kb]:", ""));
                    else if (trimmed.StartsWith("[procent]:"))
                        currentBlock.Procent = int.Parse(trimmed.Replace("[procent]:", ""));
                    else if (trimmed.StartsWith("[kompleksitet]:"))
                        currentBlock.Kompleksitet = trimmed.Replace("[kompleksitet]:", "");
                    else if (trimmed.Contains(@":\") || trimmed.Contains(@":/")) // Det ligner en sti
                        currentBlock.Paths.Add(trimmed);
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