using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace RTATimer
{
    public static class Utility
    {
        public const long hour_base = 3600000;
        public const long minute_base = 60000;
        public const long second_base = 1000;
        public static string UnsingedTimeToString(long? t)
        {
            if(t is long time)
            {
                if(time >= hour_base)
                {
                    var hour = (time / hour_base);
                    var minute = ((time % hour_base) / minute_base);
                    var second = ((time % minute_base) / second_base);
                    return
                        (hour < 10 ? "0" : "") + hour.ToString() + ":" +
                        (minute < 10 ? "0" : "") + minute.ToString() + ":" +
                        (second < 10 ? "0" : "") + second.ToString();
                }
                else
                {
                    var minute = time / minute_base;
                    var second = (time % minute_base) / second_base;
                    var mill = (time % second_base) / 10;
                    return
                        (minute < 10 ? "0" : "") + minute.ToString() + ":" +
                        (second < 10 ? "0" : "") + second.ToString() + "." +
                        (mill < 10 ? "0" : "") + mill.ToString();
                }
            }
            else
            {
                return "--:--:--";
            }
        }
        public static string DiffTimeToString(long? t)
        {
            if(t is long time)
            {
                if(time > 0)
                {
                    return "+" + UnsingedTimeToString(time);
                }
                else
                {
                    return "-" + UnsingedTimeToString(-time);
                }
            }
            return "--:--:--";
        }
        public static long? Min<T>(IEnumerable<T> enumerable, Func<T, long?> func)
        {
            long? ret = null;
            foreach(var v in enumerable)
            {
                var val = func(v);
                if (!(val > ret))
                {
                    ret = val;
                }
            }
            return ret;
        }
        public static string StrictTimeToString(long? t)
        {
            if(t is long time)
            {
                var h = time / hour_base;
                var m = (time % hour_base) / minute_base;
                var s = (time % minute_base) / second_base;
                var ml = (time % second_base);
                return
                    (h < 10 ? "0" : "") + h.ToString() + ":" +
                    (m < 10 ? "0" : "") + m.ToString() + ":" +
                    (s < 10 ? "0" : "") + s.ToString() + "." +
                    (ml < 10 ? "00" : ml < 100 ? "0" : "") + ml.ToString();
            }
            else
            {
                return "";
            }
        }

        public static long? StringToTime(string str)
        {
            var sub1 = @"((?<hour>\d+):)?";
            var sub2 = string.Format(@"({0}(?<min>\d+):)?", sub1);
            var sub3 = @"(\.(?<mill>\d*))?";
            var sub4 = string.Format(@"{0}(?<sec>\d+){1}", sub2, sub3);
            var reg = new Regex(sub4);
            var match = reg.Match(str);
            long ret = 0;
            if (match.Success)
            {
                if (match.Groups["hour"].Success)
                {
                    ret += long.Parse(match.Groups["hour"].Value) * hour_base;
                }
                if (match.Groups["min"].Success)
                {
                    ret += long.Parse(match.Groups["min"].Value) * minute_base;
                }
                if (match.Groups["mill"].Success)
                {
                    var s = match.Groups["mill"].Value;
                    if (s.Length > 3)
                    {
                        s.Remove(3, s.Length - 3);
                    }
                    while (s.Length < 3)
                    {
                        s += "0";
                    }
                    ret += long.Parse(s);
                }
                ret += long.Parse(match.Groups["sec"].Value) * second_base;
                return ret;
            }
            else
            {
                return null;
            }
        }
        public static IEnumerable<(T item, int index)> AddIndex<T>(this IEnumerable<T> cont)
        {
            int index = -1;
            foreach(var v in cont)
            {
                yield return (v, ++index);
            }
        }
        
    }

    [Serializable()]
    public class TimerColors
    {
        Color default_color;
        Color default_color_back;
        Color normal_plus;
        Color normal_plus_back;
        Color normal_minus;
        Color normal_minus_back;
        Color segbest_minus;
        Color segbest_minus_back;
        Color current_segment;
        Color current_segment_back;
        Color not_current_segment;
        Color not_current_segment_back;

        public Color DefaultColor { get => this.default_color; set => this.default_color = value; }
        public Color DefaultColorBack { get => this.default_color_back; set => this.default_color_back = value; }
        public Color NormalPlus { get => this.normal_plus; set => this.normal_plus = value; }
        public Color NormalPlusBack { get => this.normal_plus_back; set => this.normal_plus_back = value; }
        public Color NormalMinus { get => this.normal_minus; set => this.normal_minus = value; }
        public Color NormalMinusBack { get => this.normal_minus_back; set => this.normal_minus_back = value; }
        public Color SegbestMinus { get => this.segbest_minus; set => this.segbest_minus = value; }
        public Color SegbestMinusBack { get => this.segbest_minus_back; set => this.segbest_minus_back = value; }
        public Color CurrentSegment { get => this.current_segment; set => this.current_segment = value; }
        public Color CurrentSegmentBack { get => this.current_segment_back; set => this.current_segment_back = value; }
        public Color NotCurrentSegment { get => this.not_current_segment; set => this.not_current_segment = value; }
        public Color NotCurrentSegmentBack { get => this.not_current_segment_back; set => this.not_current_segment_back = value; }

        public TimerColors()
        {
        }

        public void SetDefaultColor(Label label)
        {
            label.ForeColor = this.default_color;
            label.BackColor = this.default_color_back;
        }

        public void SetNormalPlus(Label label)
        {
            label.ForeColor = this.normal_plus;
            label.BackColor = this.normal_plus_back;
        }

        public void SetNormalMinus(Label label)
        {
            label.ForeColor = this.normal_minus;
            label.BackColor = this.normal_minus_back;
        }

        public void NewSegBest(Label label)
        {
            label.ForeColor = this.segbest_minus;
            label.BackColor = this.segbest_minus_back;
        }

        public void SetCurrentSegment(Label label)
        {
            label.ForeColor = this.current_segment;
            label.BackColor = this.current_segment_back;
        }

        public void SetNotCurrentSegment(Label label)
        {
            label.ForeColor = this.not_current_segment;
            label.BackColor = this.not_current_segment_back;
        }
    }
    
}
