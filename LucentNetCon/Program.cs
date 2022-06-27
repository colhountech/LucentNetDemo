
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;

Console.WriteLine("Hello, World!");

const LuceneVersion AppLuceneVersion = LuceneVersion.LUCENE_48;

string indexName = "example_index";
string indexPath = Path.Combine(Environment.CurrentDirectory, indexName);
using var indexDir = FSDirectory.Open(indexPath);

// Create an analyzer to process the text
var analyzer = new StandardAnalyzer(AppLuceneVersion);

// Create an index writer
var indexConfig = new IndexWriterConfig(AppLuceneVersion, analyzer);
indexConfig.OpenMode = OpenMode.CREATE;                             // create/overwrite index
var writer = new IndexWriter(indexDir, indexConfig);

//Add three documents to the index
Document doc = new();
doc.Add(new TextField("titleTag", "The Apache Software Foundation - The world's largest open source foundation.", Field.Store.YES));
doc.Add(new StringField("domain", "www.apache.org/", Field.Store.YES));
writer.AddDocument(doc);

doc = new Document();
doc.Add(new TextField("title", "Powerful open source search library for .NET", Field.Store.YES));
doc.Add(new StringField("domain", "lucenenet.apache.org", Field.Store.YES));
writer.AddDocument(doc);

doc = new Document();
doc.Add(new TextField("title", "Unique gifts made by small businesses in North Carolina.", Field.Store.YES));
doc.Add(new StringField("domain", "www.giftoasis.com", Field.Store.YES));
writer.AddDocument(doc);

//Flush and commit the index data to the directory
writer.Commit();

using DirectoryReader reader = writer.GetReader(applyAllDeletes: true);
IndexSearcher searcher = new IndexSearcher(reader);

Query query = new TermQuery(new Term("domain", "lucenenet.apache.org"));
TopDocs topDocs = searcher.Search(query, n: 2);         //indicate we want the first 2 results


int numMatchingDocs = topDocs.TotalHits;
Document resultDoc = searcher.Doc(topDocs.ScoreDocs[0].Doc);  //read back first doc from results (ie 0 offset)
string title = resultDoc.Get("title");

Console.WriteLine($"Matching results: {topDocs.TotalHits}");
Console.WriteLine($"Title of first result: {title}");

Console.ReadLine();
