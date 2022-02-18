﻿using FsCheck.Xunit;
using Xunit;

namespace ComradeVanti.CSharpTools
{

    public class ChainingTests
    {

        [Property]
        public static bool MatchSomeActionIsOnlyCalledForSome(Opt<int> opt)
        {
            var executed = false;

            opt.Match(_ => executed = true, () => { });

            return executed == opt.IsSome();
        }

        [Fact]
        public static void MatchNoneActionIsOnlyCalledForNone()
        {
            var executed = false;

            Opt.None<int>().Match(_ => { }, () => executed = true);

            Assert.True(executed);
        }

        [Property]
        public static bool MatchSomeFunctionIsOnlyCalledForSome(Opt<int> opt)
        {
            var executed = false;

            _ = opt.Match(_ =>
            {
                executed = true;
                return 0;
            }, () => 0);

            return executed == opt.IsSome();
        }

        [Fact]
        public static void MatchNoneFunctionIsOnlyCalledForNone()
        {
            var result = Opt.None<int>().Match(_ => 0, () => 1);

            Assert.Equal(1, result);
        }

        [Property]
        public static bool ValueIsCorrectlyMatched(Opt<int> opt)
        {
            var result = opt.Match(it => (int?)it, () => null);
            return result != null == opt.IsSome();
        }

        [Property]
        public static bool IterActionIsOnlyCalledForSome(Opt<int> opt)
        {
            var executed = false;

            opt.Iter(_ => executed = true);

            return executed == opt.IsSome();
        }

        [Property]
        public static bool MappingFunctionIsOnlyCalledForSome(Opt<int> opt)
        {
            var executed = false;

            _ = opt.Map(_ =>
            {
                executed = true;
                return 0;
            });

            return executed == opt.IsSome();
        }

        [Fact]
        public static void MappingNoneProducesNone() =>
            Assert.True(Opt.None<int>().Map(it => it).IsNone());

        [Fact]
        public static void MappingSomeProducesSome() =>
            Assert.True(Opt.Some(1).Map(it => it).IsSome());

        [Property]
        public static bool MappingChangesTheValue(int i) =>
            Opt.Some(i).Map(it => it + 1).Get() == i + 1;

        [Property]
        public static bool BindingFunctionIsOnlyCalledForSome(Opt<int> opt)
        {
            var executed = false;

            _ = opt.Bind(_ =>
            {
                executed = true;
                return Opt.Some(0);
            });

            return executed == opt.IsSome();
        }

        [Fact]
        public static void BindingNoneProducesNone() =>
            Assert.True(Opt.None<int>().Bind(Opt.Some).IsNone());

        [Fact]
        public static void BindingSomeWithSomeFunctionProducesSome() =>
            Assert.True(Opt.Some(1).Bind(Opt.Some).IsSome());

        [Fact]
        public static void BindingSomeWithNoneFunctionProducesNone() =>
            Assert.True(Opt.Some(1).Bind(_ => Opt.None<int>()).IsNone());

        [Property]
        public static bool BindingChangesTheValue(int i) =>
            Opt.Some(i).Bind(it => Opt.Some(it + 1)).Get() == i + 1;

    }

}