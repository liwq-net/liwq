# liwq
linux,ios,windows,quicker!
liwq��һ�׻���monogame�Ŀ�ƽ̨���,ͬʱ�����ڿ������������Ϸ��
��ܲο�cocos2d����ƣ���������Ĳ��ֽ��м򻯣��Ѷ����ȥ�����ѱ���Ĳ��������򻯴����д���Ż����ܣ����WPF���UI��

##��ǰ�汾��Ҫ�����������£�
 * 1���ع����д��룬ͳһʹ��UI���Ͻ�����
 * 2��Director �� AppDelegate�ϲ�һ��"���ܹ�"
 * 3��Texture��Sprite��SpriteFrame �ϲ�
 * 4���Ż���ͼ���ܣ������ͼ��̬�ϲ����ܣ��ϳ�һ�Ŵ���ͼ���ã�����batchЧ�ܣ������ڳ�����node)��texture���棬node�ͷţ���ͼ�ͷ�
 * 5���ϵ�ԭ����menu��UIϵͳ���ϵ�Layer��ֻ����Node�Լ��ϲ��WPF���UI��ʸ���ؼ���
 * 6�����ʸ�������Լ����ٵĵ������壨ʸ������ѡ��stb ttf����xmlreader��ʽ��svg���壩
 * 7������ actions��particle system��scenes_transitions
 * 8������xnbģʽ��Դ����ģʽ��֧��ogg��png��gif,jpg��ѹ����ͼ��Դ
 * 9������¼�����ģʽ�������Ǻ㶨֡�ʣ�ʡ�磩
 * 10��actions ��Ϊbegin end������������
 * 11��֧��xaml��ƣ�֧��metro���ؼ�
 * 12��.net�����¼���ģʽ
 * 12��3Dģ��֧�֣�2.0��
 * 13��XNAVG֧�֣�2.0��
 * 14�������ֻ����й��ܣ��𶯣�gps��webview��ˮƽ�ǣ�������ã�(2.0)

 ##��������
	private int _field;
	static private int _Field;
	public int Field { get; set; }
	static public int Field { get; set; }
	
	public int Add(int count)
	{
	    return this._field++;
	}
	private int add(int count)
	{
	    return this._field++;
	}
	static public int Add(int count)
	{
	    return Class.Field++;
	}