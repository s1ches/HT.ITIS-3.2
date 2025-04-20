import 'package:flutter/material.dart';
import '../common/components/drawer.dart';
import 'details_page.dart';
import '../data/cache.dart';

class MainPage extends StatefulWidget {
  const MainPage({super.key});

  @override
  State<MainPage> createState() => _MainPageState();
}

class _MainPageState extends State<MainPage> with SingleTickerProviderStateMixin {
  late TabController _tabController;

  @override
  void initState() {
    _tabController = TabController(length: 2, vsync: this);
    super.initState();
  }

  @override
  void dispose() {
    _tabController.dispose();
    super.dispose();
  }

  void _showBottomSheet(BuildContext context) {
    showModalBottomSheet(
      context: context,
      builder: (context) => const Padding(
        padding: EdgeInsets.all(16.0),
        child: Text(
          'Lorem ipsum',
          style: TextStyle(fontSize: 18),
        ),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: const MainDrawer(),
      appBar: AppBar(
        title: const Text('Main Page'),
        bottom: TabBar(
          controller: _tabController,
          tabs: const [
            Tab(text: 'List'),
            Tab(text: 'Bottom Sheet'),
          ],
        ),
      ),
      body: TabBarView(
        controller: _tabController,
        children: [
          ListView.builder(
            itemCount: cache.length,
            itemBuilder: (context, index) {
              final item = cache[index];
              return ListTile(
                title: Text(item['title'] ?? ''),
                trailing: ElevatedButton(
                  onPressed: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(
                        builder: (context) => DetailsPage(data: item),
                      ),
                    );
                  },
                  child: const Text('Перейти'),
                ),
              );
            },
          ),
          Center(
            child: ElevatedButton(
              onPressed: () => _showBottomSheet(context),
              child: const Text('Открыть нижнее меню'),
            ),
          ),
        ],
      ),
    );
  }
}