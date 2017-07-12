using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PwC.C4.Infrastructure.Helper
{

    public class ThesaurusHelper
    {

        private readonly TrieDictionary<TrieNode> _dic;

        public ThesaurusHelper()
        {

        }

        public ThesaurusHelper(List<string> listKey)
        {
            _dic = new TrieDictionary<TrieNode>(listKey);
        }

        public ThesaurusHelper(String dirFile)
        {
            _dic = new TrieDictionary<TrieNode>(dirFile);
        }

        public bool Contains(String word)
        {

            return _dic.Contains(word);
        }

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public String Seg(String seq)
        {
            return (SegDic(seq));
        }

        /// <summary>
        /// 分词
        /// </summary>
        /// <param name="seq"></param>
        /// <returns>返回分词后的字符数组</returns>
        public String[] SegByList(String seq)
        {
            return (SegDic(seq)).Split('|');
        }


        private TrieNode GetRoot()
        {

            return _dic.GetRoot();
        }

        private String SegDic(String seq)
        {
            var segBuffer = new StringBuilder();
            int segBoundIdx = 0;
            int length = seq.Length;
            var root = GetRoot();
            var p = root;
            TrieNode pChild;
            bool isNonChinese = false;
            for (int i = 0; i < length; i++)
            {
                char c = seq[i];
                if (c == ' ')
                {
                    segBoundIdx++;
                }
                else
                {
                    if (NonChiSplit.IsCharSeperator(c))
                        isNonChinese = true;
                    if (p == root && !p.Childs.ContainsKey(Char.ToString(c)))
                    {
                        segBuffer.Append(c);
                        while (isNonChinese && ++i < length)
                        {
                            c = seq[i];
                            if (!NonChiSplit.IsCharSeperator(c))
                            {
                                i--;
                                isNonChinese = false;
                            }
                            else
                            {
                                segBuffer.Append(c);
                            }
                        }
                        if (i < length - 1)
                        {
                            segBuffer.Append("|");
                        }
                    }
                    else
                    {
                        while (p.Childs.ContainsKey(Char.ToString(c)))
                        {
                            pChild = p.Childs[Char.ToString(c)];
                            if (p == root || pChild.Bound)
                                segBoundIdx = i;
                            segBuffer.Append(c);
                            p = pChild;
                            if (++i >= length)
                                break;
                            c = seq[i];
                        }
                        if (i - 1 > segBoundIdx)
                        {
                            int start = segBuffer.Length - (i - 1 - segBoundIdx);
                            segBuffer.Remove(start, segBuffer.Length - start);
                        }

                        for (i = segBoundIdx; isNonChinese && ++i < length; )
                        {
                            c = seq[i];
                            if (!NonChiSplit.IsCharSeperator(c))
                            {
                                i--;
                                isNonChinese = false;
                            }
                            else
                            {
                                segBuffer.Append(c);
                            }
                        }
                        if (i < length - 1)
                        {
                            segBuffer.Append("|");
                        }
                        p = root;
                    }
                }
            }
            return segBuffer.ToString();
        }



    }

    internal class TrieDictionary<T> where T : TrieNode, new()
    {
        private readonly T _root = new T();

        public TrieDictionary(List<string> listKey)
        {
            ImportDict(listKey);
        }

        public TrieDictionary(String fileName)
        {
            ImportDict(fileName);
        }

        private void ImportDict(List<string> listKey)
        {
            listKey.ForEach(key => Add(key));
        }

        /// <summary>
        /// 导入词典文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private void ImportDict(string fileName)
        {
            StreamReader br = new StreamReader(fileName, Encoding.Default);
            string word;
            while ((word = br.ReadLine()) != null)
            {
                Add(word);
            }
            if (br != null)
            {
                br.Close();
            }
        }

        public void Add(String word)
        {
            T p = _root;
            p.AddWord(word, p);
        }

        public bool Contains(String word)
        {
            return GetWordEndNode(word) != null;
        }

        public T GetWordEndNode(String word)
        {
            T p = _root;
            int i;
            for (i = 0; i < word.Length; i++)
            {
                char c = word[i];
                T pChild;
                if (!p.Childs.ContainsKey(Char.ToString(c)))
                    break;

                pChild = (T)p.Childs[Char.ToString(c)];
                p = pChild;
            }
            if (i == word.Length && p.Bound)
            {
                return p;
            }
            return null;
        }


        public T GetRoot()
        {
            return _root;
        }
    }

    internal class NonChiSplit
    {
        private const String CeSeperator = "。！？：；、，（）《》【】{}“”‘’|!?:;,．()<>[]{}\"'\n\r\t ";

        /// <summary>
        /// 分词体系使用的过滤符号
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static bool IsCharSeperator(char c)
        {
            return false;
            //  return CeSeperator.IndexOf(c) != -1;
        }

    }

    internal class TrieNode
    {
        public readonly Dictionary<String, TrieNode> Childs;
        public bool Bound;
        public char Key;

        public TrieNode()
        {
            Key = '\0';
            Bound = false;
            Childs = new Dictionary<String, TrieNode>();
        }

        public TrieNode(char c)
            : this()
        {
            Key = c;
        }


        public virtual void AddWord(String word, TrieNode p)
        {
            foreach (char c in word)
            {
                TrieNode pChild;

                if (!p.Childs.ContainsKey(Char.ToString(c)))
                {
                    pChild = new TrieNode(c);

                    p.Childs.Add(Char.ToString(c), pChild);
                }
                else
                {
                    pChild = p.Childs[Char.ToString(c)];
                }
                p = pChild;
            }
            p.Bound = true;
        }
    }
}
