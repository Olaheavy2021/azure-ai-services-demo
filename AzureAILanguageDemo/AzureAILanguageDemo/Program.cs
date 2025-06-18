// See https://aka.ms/new-console-template for more information
using Azure;
using Azure.AI.TextAnalytics;

Console.WriteLine("Hello, World!");
string key = "201VGfKoPr5J1WPiEyimozMVPmTDiZYXDycvK8zi0H2rn3wUc9DEJQQJ99BFACmepeSXJ3w3AAAaACOGGQEh";
string endpoint = "https://az-language-olaheavy.cognitiveservices.azure.com/";

AzureKeyCredential credential = new AzureKeyCredential(key);
Uri languageEndpoint = new(endpoint);

var client = new TextAnalyticsClient(languageEndpoint, credential);

var sentences = new string[]
{
    "I loved the new Italian restaurant! The pasta was cooked perfectly, and the ambience was delightful",
    "This mobile app kept crashing every time I tries to log in. I'm extremely frustrated.",
    "The new movie was a visual masterpiece, but the plot was quite predictable. I expected more from the director.",
    "The customer service at the store was terrible. I waited for 30 minutes and no one helped me.",
};

//DocumentSentiment sentencesToAnalyze = client.AnalyzeSentiment("one sentence", "en");
AnalyzeSentimentResultCollection results = client.AnalyzeSentimentBatch(sentences, options: new AnalyzeSentimentOptions() { IncludeOpinionMining = true});

foreach (AnalyzeSentimentResult sentence in results)
{
    Console.WriteLine($"Sentence Sentiment: {sentence.DocumentSentiment.Sentiment}");
    Console.WriteLine($"Positive Score: {sentence.DocumentSentiment.ConfidenceScores.Positive}");
    Console.WriteLine($"Neutral Score: {sentence.DocumentSentiment.ConfidenceScores.Neutral}");
    Console.WriteLine($"Negative Score: {sentence.DocumentSentiment.ConfidenceScores.Negative}");

    foreach (var sentenceSentiment in sentence.DocumentSentiment.Sentences)
    {
        Console.WriteLine($"Sentence Aspect: {sentenceSentiment.Text}");
        Console.WriteLine($"  Sentiment: {sentenceSentiment.Sentiment}");
        Console.WriteLine($"  Positive: {sentenceSentiment.ConfidenceScores.Positive}");
        Console.WriteLine($"  Neutral: {sentenceSentiment.ConfidenceScores.Neutral}");
        Console.WriteLine($"  Negative: {sentenceSentiment.ConfidenceScores.Negative}");

        foreach (var sentenceOpinion in sentenceSentiment.Opinions)
        {
            Console.WriteLine($"Opinion Aspect: {sentenceOpinion.Target.Text}");
            Console.WriteLine($"    Sentiment: {sentenceOpinion.Target.Sentiment}");
            Console.WriteLine($"    Positive: {sentenceOpinion.Target.ConfidenceScores.Positive}");
            Console.WriteLine($"    Neutral: {sentenceOpinion.Target.ConfidenceScores.Neutral}");
            Console.WriteLine($"    Negative: {sentenceOpinion.Target.ConfidenceScores.Negative}");

            foreach (var assessmentSentiment in sentenceOpinion.Assessments)
            {
               Console.WriteLine($" Assessment: {assessmentSentiment.Text}, Value: {assessmentSentiment.Sentiment}");
                Console.WriteLine($" Positive: {assessmentSentiment.ConfidenceScores.Positive}");
                Console.WriteLine($" Neutral: {assessmentSentiment.ConfidenceScores.Neutral}");
                Console.WriteLine($" Negative: {assessmentSentiment.ConfidenceScores.Negative}");
            }
        }
    }

    Console.ReadKey();
}