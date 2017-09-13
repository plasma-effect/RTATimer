using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTATimer
{
    [Serializable()]
    public class Records
    {
        List<string> name;
        List<List<long?>> my_records;
        Dictionary<string, List<long?>> target_records;
        public Records(List<string> name)
        {
            this.name = name;
            this.my_records = new List<List<long?>>();
            this.target_records = new Dictionary<string, List<long?>>
            {
                { "best", CalcMyBest() },
                { "sobs", CalcSumOfBest() }
            };
        }

        public string Name(int index)
        {
            return index < this.name.Count ? this.name[index] : null;
        }
        public List<string> Names => this.name;
        public List<long?> GetTarget(string name)
        {
            return this.target_records[name];
        }
       
        private List<long?> CalcSumOfBest()
        {
            var list = new List<long?>
                {
                    Utility.Min(this.my_records,(lis)=>lis[0])
                };

            for (var i = 1; i < this.name.Count; ++i)
            {
                if (Utility.Min(this.my_records, (lis) => lis[i] - lis[i - 1]) is long val)
                {
                    list.Add(list[i - 1] + val);
                }
                else
                {
                    while (list.Count < this.name.Count)
                    {
                        list.Add(null);
                    }
                    break;
                }
            }

            return list;
        }
        private List<long?> CalcMyBest()
        {
            var list = Enumerable.Repeat<long?>(null, this.name.Count).ToList();
            foreach (var v in this.my_records)
            {
                if (v.Last() is long val && !(val > list.Last()))
                {
                    list = v;
                }
            }

            return list;
        }

        public void AddRecord(List<long?> list)
        {
            if (list.Count < this.name.Count)
            {
                list.AddRange(Enumerable.Repeat<long?>(null, this.name.Count - list.Count));
            }
            else if (list.Count > this.name.Count)
            {
                list.RemoveRange(this.name.Count, list.Count - this.name.Count);
            }
            this.my_records.Add(list);
            this.target_records.Remove("best");
            this.target_records.Remove("sobs");
            this.target_records.Add("best", CalcMyBest());
            this.target_records.Add("sobs", CalcSumOfBest());
        }
        public void AddTarget(string name, List<long?> list)
        {
            if (list.Count < this.name.Count)
            {
                list.AddRange(Enumerable.Repeat<long?>(null, this.name.Count - list.Count));
            }
            else if (list.Count > this.name.Count)
            {
                list.RemoveRange(this.name.Count, list.Count - this.name.Count);
            }
            if (this.target_records.ContainsKey(name))
            {
                this.target_records.Remove(name);
            }
            this.target_records.Add(name, list);
        }

        public int Size => this.name.Count;

        public List<List<long?>> MyRecords => this.my_records;
        public List<long?> LastRecords => this.my_records.Last();
        public Dictionary<string, List<long?>> TargetRecords => this.target_records;

        public bool ContainsTarget(string name)
        {
            return this.target_records.ContainsKey(name);
        }
    }
}
