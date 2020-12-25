using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dena.CodeAnalysis.Testing
{
    public static class SetAssert
    {
        public static void AreEqual<T>(ISet<T> a, ISet<T> b)
        {
            // 含まれている要素が一致するなら何もしない
            if (a.SetEquals(b)) { return; }
            var leftExtra = new HashSet<T>(a); // このすぐ後で破壊的操作が入るのでコピーしておく
            var rightExtra = new HashSet<T>(b);
            var common = new HashSet<T>(a);
            leftExtra.ExceptWith(b); // leftExtra に b に含まれていない要素だけが残る（数学でいう集合の引き算）
            rightExtra.ExceptWith(a); // 右側のみに含まれている要素もとっておく
            common.IntersectWith(b); // 左と右の両方に入っているやつ
            var builder = new StringBuilder(); // 失敗時のメッセージを組み立てていくためのやつ
            foreach (var l in leftExtra) {
                builder.Append($"\n- {l}");
            } // 左にしか入っていない奴の頭に - をつける
            foreach (var r in rightExtra) {
                builder.Append($"\n+ {r}");
            } // 右にしか入っていない奴の頭に + をつける
            foreach (var x in common) {
                builder.Append($"\n  {x}");
            } // 両方に入っている奴には何もつけない
            Assert.Fail(builder.ToString()); // + と - とかでいい感じの見た目のメッセージで失敗させる
        }
    }
}
