import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../view_models/single_article_view_model.dart';

class ArticlePage extends StatelessWidget {
  final int articleId;

  const ArticlePage({super.key, required this.articleId});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Article')),
      body: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Consumer<SingleArticleViewModel>(
          builder: (context, viewModel, child) {
            if (viewModel.article == null) {
              viewModel.fetchArticle(articleId);
              return const Center(child: CircularProgressIndicator());
            } else {
              final article = viewModel.article!;
              return SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      article.title,
                      style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
                    ),
                    const SizedBox(height: 10),
                    Text(
                      article.body,
                      style: const TextStyle(fontSize: 16),
                    ),
                  ],
                ),
              );
            }
          },
        ),
      ),
    );
  }
}