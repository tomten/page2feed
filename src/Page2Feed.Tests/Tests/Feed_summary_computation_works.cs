using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Page2Feed.Core.Services;

// ReSharper disable InconsistentNaming

namespace Page2Feed.Tests.Tests
{

    [TestFixture]
    public class Feed_summary_computation_works
    {

        [Test]
        public void Trim_same_start_keeps_words_intact()
        {

            // Arr
            var s1 = "Räksmörgås är gott.";
            var s2 = "Räksmörgåsbord är gott.";

            var t1 = "En räksmörgås är gott.";
            var t2 = "En röksmörgås är gott.";

            var feedService =
                new FeedService(
                    null,
                    null
                    );

            // Act
            var s3 = feedService.TrimSameStart(s1, s2);
            var t3 = feedService.TrimSameStart(t1, t2);

            // Ass
            Assert.AreEqual("Räksmörgåsbord är gott.", s3);
            Assert.AreEqual("röksmörgås är gott.", t3);

        }

        [Test]
        public void Trim_same_start_works()
        {

            // Arr
            var s1 = "asdf Lorem Hejan i fejan på dejan.";
            var s2 = "asdf Lorem Hejan i den gamla block-skallen.";

            var t1 = "asdf Lorem Hejan i den gamla block-skallen.";
            var t2 = "asdf Lorem Hejan i fejan på dejan.";

            var u1 = "Hip to the hop";
            var u2 = "Hip to the hop";

            var v1 = "Hip to the hop";
            var v2 = "Hip to the ho";

            var w1 = "Hip to the ho";
            var w2 = "Hip to the hop";

            var feedService =
                new FeedService(
                    null,
                    null
                    );

            // Act
            var s3 = feedService.TrimSameStart(s1, s2);
            var t3 = feedService.TrimSameStart(t1, t2);
            var u3 = feedService.TrimSameStart(u1, u2);
            var v3 = feedService.TrimSameStart(v1, v2);
            var w3 = feedService.TrimSameStart(w1, w2);

            // Ass
            Assert.AreEqual("den gamla block-skallen.", s3);
            Assert.AreEqual("fejan på dejan.", t3);
            Assert.AreEqual("", u3);
            Assert.AreEqual("ho", v3);
            Assert.AreEqual("hop", w3);

        }

        [Test]
        public async Task Feed_summary_is_computed_correctly()
        {

            // Arrange
            var testsPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Tests"
            );
            var html1 = File.ReadAllText(
                Path.Combine(
                    testsPath,
                    "sample.html"
                )
            );
            var html2 = File.ReadAllText(
                Path.Combine(
                    testsPath,
                    "sample2.html"
                )
            );

            // Act
            var summary =
                await new FeedService(
                        null,
                        null
                    )
                    .MakeSummary(
                        html1,
                        html2
                        );

            // Assert
            Assert.IsTrue(summary.StartsWith("Här ser ni veckans bokstav"));

        }

    }

}
