import 'package:cleanarchitecture/presentation/article/view_models/articles_list_view_model.dart';
import 'package:cleanarchitecture/presentation/article/pages/articles_page.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'internal/application.dart';
import 'presentation/article/view_models/single_article_view_model.dart';
import 'presentation/article/pages/article_page.dart';

void main() {
  Application.init();
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        ChangeNotifierProvider(create: (_) => SingleArticleViewModel()),
        ChangeNotifierProvider(create: (_) => ArticlesListViewModel(getIt())),
      ],
      child: MaterialApp(
        title: 'Flutter Clean Architecture',
        theme: ThemeData(primarySwatch: Colors.blue),
        home: const HomeScreen(),
      ),
    );
  }
}

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Home')),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            ElevatedButton(
              onPressed: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const ArticlePage(articleId: 1,)),
              ),
              child: const Text('Load Single Article'),
            ),
            ElevatedButton(
              onPressed: () => Navigator.push(
                context,
                MaterialPageRoute(builder: (_) => const ArticlesPage()),
              ),
              child: const Text('Load Articles List'),
            ),
          ],
        ),
      ),
    );
  }
}