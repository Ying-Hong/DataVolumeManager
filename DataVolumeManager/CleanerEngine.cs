using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataVolumeManager
{
    public static class CleanerEngine
    {
        public static void Run(IEnumerable<FolderRule> rules)
        {
            foreach (var rule in rules.Where(r => r.Enabled))
            {
                if (!Directory.Exists(rule.Path)) continue;
                DateTime expire = DateTime.Now.AddDays(-rule.KeepDays);
                var extSet = ParseExtensions(rule.Extensions);
                Scan(rule.Path, rule, expire, extSet, 0);
            }
        }

        private static void Scan(string dir, FolderRule rule, DateTime expire, HashSet<string> extSet, int depth)
        {
            if (rule.MaxDepth > 0 && depth > rule.MaxDepth) return;

            foreach (var file in Directory.GetFiles(dir))
            {
                try
                {
                    var fi = new FileInfo(file);
                    if (extSet.Any() && !extSet.Contains(fi.Extension.ToLower())) continue;

                    if (fi.LastWriteTime < expire)
                    {
                        if (rule.DryRun)
                            LogService.Write("[DRY] " + file);
                        else
                        {
                            fi.Delete();
                            LogService.Write("[DEL] " + file);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.Write("[ERR] " + file + " " + ex.Message);
                }
            }

            if (!rule.Recursive) return;

            foreach (var sub in Directory.GetDirectories(dir))
                Scan(sub, rule, expire, extSet, depth + 1);
        }

        private static HashSet<string> ParseExtensions(string ext)
        {
            if (string.IsNullOrWhiteSpace(ext)) return new HashSet<string>();
            return ext.Split(';').Select(e => e.Trim().ToLower()).Where(e => e.StartsWith("."))
                      .ToHashSet();
        }
    }
}
