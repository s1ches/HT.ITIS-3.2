import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import '../view_models/articles_list_view_model.dart';

class ArticlesPage extends StatelessWidget {
  const ArticlesPage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Articles')),
      body: const ArticlesListView(),
    );
  }
}

class ArticlesListView extends StatefulWidget {
  const ArticlesListView({super.key});

  @override
  State<ArticlesListView> createState() => _ArticlesListViewState();
}

class _ArticlesListViewState extends State<ArticlesListView> {
  late ScrollController _scrollController;

  @override
  void initState() {
    super.initState();
    _scrollController = ScrollController()
      ..addListener(_onScroll);
  }

  void _onScroll() {
    if (_scrollController.position.pixels >= _scrollController.position.maxScrollExtent - 200) {
      context.read<ArticlesListViewModel>().onScrollEnd();
    }
  }

  @override
  Widget build(BuildContext context) {
    final viewModel = context.watch<ArticlesListViewModel>();

    return ListView.builder(
      controller: _scrollController,
      itemCount: viewModel.articles.length + (viewModel.isLoading ? 1 : 0),
      itemBuilder: (context, index) {
        if (index < viewModel.articles.length) {
          final article = viewModel.articles[index];
          return ListTile(
            title: Text(article.title),
            subtitle: Text(article.body),
          );
        } else {
          return const Padding(
            padding: EdgeInsets.symmetric(vertical: 16),
            child: Center(child: CircularProgressIndicator()),
          );
        }
      },
    );
  }

  @override
  void dispose() {
    _scrollController.dispose();
    super.dispose();
  }
}